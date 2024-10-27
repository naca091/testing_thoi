    using BusinessLogic;
    using DataAccess.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    namespace WarehouseManagementApp
    {
        public partial class EditLotWindow : Window
        {
            private readonly LotService _lotService;
            private readonly ProductService _productService;
            private readonly PartnerService _partnerService;
            private ObservableCollection<LotDetail> _lotDetails;
            private Lot _currentLot;

            public EditLotWindow(LotService lotService, ProductService productService, PartnerService partnerService, int lotId)
            {
                InitializeComponent();
                _lotService = lotService ?? throw new ArgumentNullException(nameof(lotService));
                _productService = productService ?? throw new ArgumentNullException(nameof(productService));
                _partnerService = partnerService ?? throw new ArgumentNullException(nameof(partnerService));
                _lotDetails = new ObservableCollection<LotDetail>();
                ProductsDataGrid.ItemsSource = _lotDetails;
                LoadLot(lotId);
                LoadPartners();
                LoadProducts();
            }
            private bool ValidateLotCode()
            {
                var lotCode = LotCodeTextBox.Text.Trim();
                var regex = new Regex(@"^lot\d{3}$", RegexOptions.IgnoreCase);

                if (!regex.IsMatch(lotCode))
                {
                    MessageBox.Show("LotCode must start with 'lot' or 'LOT' followed by 3 digits.", "Invalid LotCode", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LotCodeTextBox.Focus();
                    return false;
                }

                return true;
            }

            private void LoadLot(int lotId)
            {
                _currentLot = _lotService.GetLotById(lotId);
                if (_currentLot != null)
                {
                    LotCodeTextBox.Text = _currentLot.LotCode;
                    DateInPicker.SelectedDate = _currentLot.DateIn;
                    NoteTextBox.Text = _currentLot.Note;
                    PartnerComboBox.SelectedValue = _currentLot.PartnerId;

                    foreach (var detail in _currentLot.LotDetails)
                    {
                        _lotDetails.Add(detail);
                    }
                }
            }

            private void LoadPartners()
            {
                try
                {
                    PartnerComboBox.ItemsSource = _partnerService.GetAllPartners();
                    PartnerComboBox.DisplayMemberPath = "Name";
                    PartnerComboBox.SelectedValuePath = "PartnerId";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading partners: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            private void LoadProducts()
            {
                ProductComboBox.ItemsSource = _productService.GetAllProducts();
                ProductComboBox.DisplayMemberPath = "Name";
                ProductComboBox.SelectedValuePath = "ProductId";
            }

            private void RemoveProduct_Click(object sender, RoutedEventArgs e)
            {
                if (sender is Button button && button.DataContext is LotDetail item)
                {
                    _lotDetails.Remove(item);
                }
            }

            private void AddProduct_Click(object sender, RoutedEventArgs e)
            {
                if (ProductComboBox.SelectedItem is Product selectedProduct &&
                    int.TryParse(QuantityTextBox.Text, out int quantity) &&
                    quantity > 0)
                {
                    var existingItem = _lotDetails.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        _lotDetails.Add(new LotDetail
                        {
                            ProductId = selectedProduct.ProductId,
                            Quantity = quantity,
                            PartnerId = (int)PartnerComboBox.SelectedValue,
                            Status = 1,
                            Product = selectedProduct
                        });
                    }
                    ProductComboBox.SelectedIndex = -1;
                    QuantityTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Please select a product and enter a valid quantity.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }




            private void UpdateLot_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    // Validate LotCode before updating
                    if (!ValidateLotCode())
                    {
                        return; // Exit if validation fails
                    }

                    // Assign the updated values
                    _currentLot.LotCode = LotCodeTextBox.Text;
                    _currentLot.PartnerId = (int)PartnerComboBox.SelectedValue;
                    _currentLot.DateIn = DateInPicker.SelectedDate.Value;
                    _currentLot.Note = NoteTextBox.Text;

                    // Create a new list for LotDetails
                    var updatedLotDetails = new List<LotDetail>();

                    foreach (var item in _lotDetails)
                    {
                        // Create a new LotDetail instead of using the item directly
                        var newDetail = new LotDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            PartnerId = item.PartnerId,
                            Status = item.Status,
                            LotId = _currentLot.LotId
                        };
                        updatedLotDetails.Add(newDetail);
                    }

                    // Update the lot with the new list of LotDetails
                    _lotService.UpdateLot(_currentLot, updatedLotDetails);
                    MessageBox.Show("Lot edit successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating lot: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
