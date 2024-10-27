﻿using BusinessLogic;
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