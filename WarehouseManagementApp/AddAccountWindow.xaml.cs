using System;
using System.Windows;
using System.Windows.Controls;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class AddAccountWindow : Window
    {
        private readonly AccountService _accountService;

        public AddAccountWindow(AccountService accountService)
        {
            InitializeComponent();
            _accountService = accountService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs before adding
            if (!ValidateInput()) return;

            // Create a new Account object
            var newAccount = new Account
            {
                AccountCode = AccountCodeTextBox.Text,
                Email = EmailTextBox.Text,
                Name = NameTextBox.Text,
                Password = PasswordBox.Password,
                Role = int.Parse(((ComboBoxItem)RoleComboBox.SelectedItem).Tag.ToString()),
                Phone = PhoneTextBox.Text,
                Status = int.Parse(((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString())
            };

            try
            {
                _accountService.AddAccount(newAccount);
                MessageBox.Show("Account added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving account: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(AccountCodeTextBox.Text) || !System.Text.RegularExpressions.Regex.IsMatch(AccountCodeTextBox.Text, @"^ACC\d{3,}$"))
            {
                MessageBox.Show("Account Code must start with 'ACC' followed by at least 3 digits.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Email is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Password is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
