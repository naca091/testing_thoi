using System;
using System.Windows;
using System.Windows.Controls;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class AddPartnerWindow : Window
    {
        private readonly PartnerService _partnerService;

        public AddPartnerWindow(PartnerService partnerService)
        {
            InitializeComponent();
            _partnerService = partnerService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtPartnerCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newPartner = new Partner
            {
                PartnerCode = txtPartnerCode.Text,
                Name = txtName.Text,
                Status = int.Parse(((ComboBoxItem)cmbStatus.SelectedItem).Tag.ToString())
            };

            try
            {
                _partnerService.AddPartner(newPartner);
                MessageBox.Show("Partner added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding partner: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
