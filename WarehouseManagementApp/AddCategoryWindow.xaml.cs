using BusinessLogic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WarehouseManagementApp
{
    public partial class AddCategoryWindow : Window
    {
        private readonly CategoryService _categoryService;

        public AddCategoryWindow(CategoryService categoryService)
        {
            InitializeComponent();
            _categoryService = categoryService;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string categoryCode = CategoryCodeTextBox.Text;
                string name = NameTextBox.Text;
                int status = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Tag?.ToString() == "1" ? 1 : 0;

                // Validate CategoryCode
                if (string.IsNullOrEmpty(categoryCode) || !System.Text.RegularExpressions.Regex.IsMatch(categoryCode, @"^CAT\d{3,}$"))
                {
                    MessageBox.Show("Category Code must start with 'CAT' followed by at least 3 digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate Name
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (name.Length < 3)
                {
                    MessageBox.Show("Name must be at least 3 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate Status selection
                if (StatusComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // If all validations pass, create the category
                _categoryService.CreateCategory(categoryCode, name, status);

                MessageBox.Show("Category added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; // Closes the window and indicates success
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
