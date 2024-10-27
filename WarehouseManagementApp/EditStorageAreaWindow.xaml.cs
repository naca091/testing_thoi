using System;
using System.Windows;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class EditStorageAreaWindow : Window
    {
        private readonly StorageAreaService _storageAreaService;
        private readonly StorageArea _storageArea;

        public EditStorageAreaWindow(StorageAreaService storageAreaService, StorageArea storageArea)
        {
            InitializeComponent();
            _storageAreaService = storageAreaService;
            _storageArea = storageArea;

            AreaCodeTextBox.Text = _storageArea.AreaCode;
            AreaNameTextBox.Text = _storageArea.AreaName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newAreaName = AreaNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newAreaName))
            {
                MessageBox.Show("Area Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _storageArea.AreaName = newAreaName;
                _storageAreaService.UpdateStorageArea(_storageArea);
                MessageBox.Show("Storage area updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating storage area: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
