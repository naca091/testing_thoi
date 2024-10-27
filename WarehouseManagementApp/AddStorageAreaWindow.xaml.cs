using System;
using System.Text.RegularExpressions;
using System.Windows;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class AddStorageAreaWindow : Window
    {
        private readonly StorageAreaService _storageAreaService;

        public AddStorageAreaWindow(StorageAreaService storageAreaService)
        {
            InitializeComponent();
            _storageAreaService = storageAreaService;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string areaCode = AreaCodeTextBox.Text.Trim();
            string areaName = AreaNameTextBox.Text.Trim();

            // Validate AreaCode
            if (!Regex.IsMatch(areaCode, @"^AREA\d{3,}$"))
            {
                MessageBox.Show("Area Code must start with 'AREA' followed by at least 3 digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(areaName))
            {
                MessageBox.Show("Area Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var storageArea = new StorageArea
                {
                    AreaCode = areaCode,
                    AreaName = areaName,
                    Status = 1  // Active status
                };

                _storageAreaService.AddStorageArea(storageArea);
                MessageBox.Show("Storage area added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding storage area: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
