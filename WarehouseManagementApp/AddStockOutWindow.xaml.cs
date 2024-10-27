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
    /// <summary>
    /// Interaction logic for AddStockOutWindow.xaml
    /// </summary>
    public partial class AddStockOutWindow : Window
    {
        private readonly StockOutService _stockOutService;
        private readonly ProductService _productService;
        private readonly PartnerService _partnerService;
        private ObservableCollection<StockOutDetail> _stockOutDetails;

        public AddStockOutWindow(StockOutService stockOutService, ProductService productService, PartnerService partnerService)
        {
            InitializeComponent();
            _stockOutService = stockOutService;
            _productService = productService;
            _partnerService = partnerService;
            _stockOutDetails = new ObservableCollection<StockOutDetail>();
            ProductsDataGrid.ItemsSource = _stockOutDetails;
            LoadPartners();
            LoadProducts();
            DateOutPicker.SelectedDate = DateTime.Now;
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
                var existingItem = _stockOutDetails.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    _stockOutDetails.Add(new StockOutDetail
                    {
                        ProductId = selectedProduct.ProductId,
                        Quantity = quantity,
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

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is StockOutDetail item)
            {
                _stockOutDetails.Remove(item);
            }
        }

        private void AddStockOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PartnerComboBox.SelectedItem == null ||
                    DateOutPicker.SelectedDate == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_stockOutDetails.Count == 0)
                {
                    MessageBox.Show("Please add at least one product to the stock out.", "No Products", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newStockOut = new StockOut
                {
                    PartnerId = (int)PartnerComboBox.SelectedValue,
                    DateOut = DateOutPicker.SelectedDate.Value,
                    Note = NoteTextBox.Text,
                    Status = 1,
                    AccountId = 1 // You might want to get this from the logged-in user
                };

                _stockOutService.AddStockOutWithDetails(newStockOut, _stockOutDetails.ToList());

                MessageBox.Show("Stock Out added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding stock out: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
