using BusinessLogic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            CategoryComboBox.DisplayMemberPath = "Name"; // Thay đổi theo thuộc tính đúng của bạn
            CategoryComboBox.SelectedValuePath = "CategoryId"; // Thay đổi theo thuộc tính đúng của bạn
        }

        private void LoadStorageAreas()
        {
            StorageAreaComboBox.ItemsSource = _storageAreaService.GetAllStorageAreas();
            StorageAreaComboBox.DisplayMemberPath = "Name"; // Thay đổi theo thuộc tính đúng của bạn
            StorageAreaComboBox.SelectedValuePath = "AreaId"; // Thay đổi theo thuộc tính đúng của bạn
        }

        private void LoadProductDetails()
        {
            var product = _productService.GetProductById(_productId);
            if (product != null)
            {
                ProductCodeTextBox.Text = product.ProductCode; // Khóa chính không thay đổi
                NameTextBox.Text = product.Name;
                QuantityTextBox.Text = product.Quantity.ToString();
                StatusComboBox.SelectedValue = product.Status;

                // Đặt giá trị cho Category và Storage Area
                CategoryComboBox.SelectedValue = product.CategoryId;
                StorageAreaComboBox.SelectedValue = product.AreaId;
            }
            else
            {
                MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra đầu vào
            if (!ValidateInput()) return;

            var updatedProduct = new Product
            {
                ProductId = _productId, // Sử dụng ID hiện tại để cập nhật
                ProductCode = ProductCodeTextBox.Text,
                Name = NameTextBox.Text,
                Quantity = int.Parse(QuantityTextBox.Text),
                Status = int.Parse(((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString()),
                CategoryId = ((Category)CategoryComboBox.SelectedItem).CategoryId,
                AreaId = ((StorageArea)StorageAreaComboBox.SelectedItem).AreaId
            };

            try
            {
                _productService.UpdateProduct(updatedProduct);
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; // Đóng cửa sổ với kết quả true
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
            DialogResult = false; // Đóng cửa sổ mà không lưu
        }
    }
}
