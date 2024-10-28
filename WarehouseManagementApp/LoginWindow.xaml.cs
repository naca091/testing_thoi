using System.Windows;
using BusinessLogic;
using DataAccess.Models;

namespace WarehouseManagementApp
{
    public partial class LoginWindow : Window
    {
        private readonly AccountService _accountService;
        public static Account? LoggedInAccount { get; private set; } // Add this static property

        // Parameterless constructor
        public LoginWindow()
        {
            InitializeComponent();
            var context = new PRN221_Warehouse(); // Initialize your DbContext here
            _accountService = new AccountService(context); // Pass the context to AccountService
        }


        // Constructor with AccountService parameter
        public LoginWindow(AccountService accountService)
        {
            InitializeComponent();
            _accountService = accountService;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Email and Password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var account = _accountService.GetAccountByEmailAndPassword(email, password);
            if (account != null)
            {
                if (account.Status == 1)
                {
                    LoggedInAccount = account; // Store the logged in account
                    MessageBox.Show("Login Success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Your Account is Banned from System.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid Email or Password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
