using System;
using System.Windows;
using System.Windows.Controls;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class EditProductWindow : Window
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly StorageAreaService _storageAreaService;
        private readonly int _productId;

        public EditProductWindow(ProductService productService, CategoryService categoryService, StorageAreaService storageAreaService, int productId)
        {
            InitializeComponent();
            _productService = productService;
            _categoryService = categoryService;
            _storageAreaService = storageAreaService;
            _productId = productId;

            LoadCategories();
            LoadStorageAreas();
            LoadProductDetails();
        }

        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource = _categoryService.GetAllCategories();
        }

        private void LoadStorageAreas()
        {
            StorageAreaComboBox.ItemsSource = _storageAreaService.GetAllStorageAreas();
        }

        private void LoadProductDetails()
        {
            var product = _productService.GetProductByID(_productId);
            if (product == null)
            {
                MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }

            ProductCodeTextBox.Text = product.ProductCode;
            NameTextBox.Text = product.Name;
            CategoryComboBox.SelectedValue = product.CategoryId;
            StorageAreaComboBox.SelectedValue = product.AreaId;
            QuantityTextBox.Text = product.Quantity.ToString();

            if (product.Status == 0)
                StatusComboBox.SelectedIndex = 0; // Inactive
            else
                StatusComboBox.SelectedIndex = 1; // Active
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Input validation
            if (!ValidateInput()) return;

            var updatedProduct = new Product
            {
                ProductId = _productId,
                CategoryId = ((Category)CategoryComboBox.SelectedItem).CategoryId,
                AreaId = ((StorageArea)StorageAreaComboBox.SelectedItem).AreaId,
                ProductCode = ProductCodeTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                Quantity = int.Parse(QuantityTextBox.Text.Trim()),
                Status = int.Parse(((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString())
            };

            try
            {
                _productService.UpdateProduct(updatedProduct);
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (CategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (StorageAreaComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a storage area.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(ProductCodeTextBox.Text) ||
                !System.Text.RegularExpressions.Regex.IsMatch(ProductCodeTextBox.Text, @"^PROD\d{3,}$"))
            {
                MessageBox.Show("Product Code must follow the format 'PROD' followed by at least 3 digits.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter a product name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a non-negative integer.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
