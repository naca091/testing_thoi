using BusinessLogic;
using DataAccess.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;

namespace WarehouseManagementApp
{
    public partial class AddLotWindow : Window
    {
        private readonly LotService _lotService;
        private readonly ProductService _productService;
        private readonly PartnerService _partnerService;
        private ObservableCollection<LotDetail> _lotDetails;

        public AddLotWindow(LotService lotService, ProductService productService, PartnerService partnerService)
        {
            InitializeComponent();
            _lotService = lotService;
            _productService = productService;
            _partnerService = partnerService;
            _lotDetails = new ObservableCollection<LotDetail>();
            ProductsDataGrid.ItemsSource = _lotDetails;
            LoadPartners();
            LoadProducts();
            DateInPicker.SelectedDate = DateTime.Today;
        }


        private void LoadPartners()
        {
            PartnerComboBox.ItemsSource = _partnerService.GetAllPartners();
            PartnerComboBox.DisplayMemberPath = "Name";
            PartnerComboBox.SelectedValuePath = "PartnerId";
        }

        private void LoadProducts()
        {
            ProductComboBox.ItemsSource = _productService.GetAllProducts();
            ProductComboBox.DisplayMemberPath = "Name";
            ProductComboBox.SelectedValuePath = "ProductId";
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
                        // Không gán Product nữa
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


        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is LotDetail item)
            {
                _lotDetails.Remove(item);
            }
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

        private void AddLot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate LotCode
                if (!ValidateLotCode())
                {
                    return; // Exit if validation fails
                }

                // Other field validation
                if (string.IsNullOrWhiteSpace(LotCodeTextBox.Text) ||
                    PartnerComboBox.SelectedItem == null ||
                    DateInPicker.SelectedDate == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_lotDetails.Count == 0)
                {
                    MessageBox.Show("Please add at least one product to the lot.", "No Products", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newLot = new Lot
                {
                    LotCode = LotCodeTextBox.Text,
                    PartnerId = (int)PartnerComboBox.SelectedValue,
                    DateIn = DateInPicker.SelectedDate.Value,
                    Note = NoteTextBox.Text,
                    Status = 1,
                    AccountId = 1 // You might want to get this from the logged-in user
                };

                _lotService.AddLotWithDetails(newLot, _lotDetails.ToList());

                MessageBox.Show("Lot added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding lot: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}