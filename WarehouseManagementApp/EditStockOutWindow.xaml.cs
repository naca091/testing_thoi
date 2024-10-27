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
    /// Interaction logic for EditStockOutWindow.xaml
    /// </summary>
    public partial class EditStockOutWindow : Window
    {
        private readonly StockOutService _stockOutService;
        private readonly ProductService _productService;
        private readonly PartnerService _partnerService;
        private ObservableCollection<StockOutDetail> _stockOutDetails;
        private StockOut _currentStockOut;

        public EditStockOutWindow(StockOutService stockOutService, ProductService productService, PartnerService partnerService, int stockOutId)
        {
            InitializeComponent();
            _stockOutService = stockOutService;
            _productService = productService;
            _partnerService = partnerService;
            _stockOutDetails = new ObservableCollection<StockOutDetail>();
            StockOutDetailsDataGrid.ItemsSource = _stockOutDetails;
            LoadStockOut(stockOutId);
            LoadPartners();
            LoadProducts();
        }

        private void LoadStockOut(int stockOutId)
        {
            _currentStockOut = _stockOutService.GetStockOutById(stockOutId);
            if (_currentStockOut != null)
            {
                StockOutIdTextBox.Text = _currentStockOut.StockOutId.ToString();
                PartnerComboBox.SelectedValue = _currentStockOut.PartnerId;
                DateOutPicker.SelectedDate = _currentStockOut.DateOut;
                NoteTextBox.Text = _currentStockOut.Note;

                foreach (var detail in _currentStockOut.StockOutDetails)
                {
                    _stockOutDetails.Add(detail);
                }
            }
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

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is StockOutDetail item)
            {
                _stockOutDetails.Remove(item);
            }
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
                        StockOutId = _currentStockOut.StockOutId,
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

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentStockOut.PartnerId = (int)PartnerComboBox.SelectedValue;
                _currentStockOut.DateOut = DateOutPicker.SelectedDate.Value;
                _currentStockOut.Note = NoteTextBox.Text;

                _currentStockOut.StockOutDetails.Clear();
                foreach (var item in _stockOutDetails)
                {
                    _currentStockOut.StockOutDetails.Add(item);
                }

                _stockOutService.UpdateStockOut(_currentStockOut);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock out: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
