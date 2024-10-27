using System;
using System.Windows;
using System.Windows.Controls;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class EditPartnerWindow : Window
    {
        private readonly PartnerService _partnerService;
        private readonly Partner _partner;

        public EditPartnerWindow(PartnerService partnerService, Partner partner)
        {
            InitializeComponent();
            _partnerService = partnerService;
            _partner = partner;
            LoadPartnerDetails();
        }

        private void LoadPartnerDetails()
        {
            txtPartnerId.Text = _partner.PartnerId.ToString();
            txtPartnerCode.Text = _partner.PartnerCode;
            txtName.Text = _partner.Name;
            cmbStatus.SelectedIndex = _partner.Status;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartnerCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _partner.PartnerCode = txtPartnerCode.Text;
            _partner.Name = txtName.Text;
            _partner.Status = int.Parse(((ComboBoxItem)cmbStatus.SelectedItem).Tag.ToString());

            try
            {
                _partnerService.UpdatePartner(_partner);
                MessageBox.Show("Partner updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating partner: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
