using System.Windows;
using DataAccess.Models;
using BusinessLogic;

namespace WarehouseManagementApp
{
    public partial class ProductDetailsWindow : Window
    {
        private readonly ProductService _productService;
        private readonly int _productId;

        public ProductDetailsWindow(ProductService productService, int productId)
        {
            InitializeComponent();
            _productService = productService;
            _productId = productId;

            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            try
            {
                var product = _productService.GetProductById(_productId);
                if (product != null)
                {
                    ProductCodeTextBox.Text = product.ProductCode;
                    NameTextBox.Text = product.Name;
                    QuantityTextBox.Text = product.Quantity.ToString();
                    CategoryTextBox.Text = product.Category.Name; // Assuming Category has a Name property
                    StorageAreaTextBox.Text = product.Area.AreaName; // Assuming StorageArea has a Name property
                    StatusTextBox.Text = product.Status == 1 ? "Active" : "Inactive";
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
