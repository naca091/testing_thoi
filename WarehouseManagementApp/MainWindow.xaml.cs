using BusinessLogic;
using DataAccess;
using DataAccess.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarehouseManagementApp
{
    public partial class MainWindow : Window
    {
        private readonly LotService _lotService;
        private readonly StockOutService _stockOutService;
        private readonly ProductService _productService;
        private readonly PartnerService _partnerService;
        private readonly CategoryService _categoryService;
        private readonly StorageAreaService _storageAreaService;




        public MainWindow()
        {
            InitializeComponent();
            var context = new PRN221_Warehouse();

            _lotService = new LotService(context);
            _productService = new ProductService(context);
            _partnerService = new PartnerService(context);
            _categoryService = new CategoryService(new CategoryDAO(context)); // Initialize CategoryService

            var stockOutDAO = new StockOutDAO(context);
            _stockOutService = new StockOutService(stockOutDAO);
            _storageAreaService = new StorageAreaService(new StorageAreaDAO(context));

            LoadStorageAreas();
            LoadProducts();

            LoadLots();
            LoadStockOuts();
            LoadCategories(); // Load categories on startup
        }
        private void LoadCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                CategoryDataGrid.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load initial data when the window is loaded
        }

        private void LoadLots()
        {
            var lots = _lotService.GetAllLots();
            LotDataGrid.ItemsSource = lots;
        }





        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var selectedTab = (TabControl)sender;
                var selectedIndex = selectedTab.SelectedIndex;
                // Xử lý khi tab được chọn
            }
        }

        private void AddLot_Click(object sender, RoutedEventArgs e)
        {
            var addLotWindow = new AddLotWindow(_lotService, new ProductService(new PRN221_Warehouse()), new PartnerService(new PRN221_Warehouse()));
            addLotWindow.Owner = this;
            if (addLotWindow.ShowDialog() == true)
            {
                LoadLots(); // Refresh the lots list
            }
        }

        private void EditLot_Click(object sender, RoutedEventArgs e)
        {
            if (LotDataGrid.SelectedItem is Lot selectedLot)
            {
                try
                {
                    // Đảm bảo rằng các service không phải là null
                    if (_lotService == null)
                        throw new InvalidOperationException("LotService is not initialized.");
                    if (_productService == null)
                        throw new InvalidOperationException("ProductService is not initialized.");
                    if (_partnerService == null)
                        throw new InvalidOperationException("PartnerService is not initialized.");

                    var editLotWindow = new EditLotWindow(_lotService, _productService, _partnerService, selectedLot.LotId);
                    editLotWindow.Owner = this;
                    if (editLotWindow.ShowDialog() == true)
                    {
                        LoadLots(); // Refresh the lots list
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening Edit Lot window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a lot to edit.", "No Lot Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteLot_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý xóa Lot
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (LotDataGrid.SelectedItem is Lot selectedLot)
            {
                var lotDetailsWindow = new LotDetailsWindow(_lotService, selectedLot.LotId);
                lotDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a lot to view details.", "No Lot Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "Search by Product Name")
            {
                MessageBox.Show("Please enter a product name to search.", "Search Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var searchResults = _lotService.SearchLotsByProductName(searchText);
            LotDataGrid.ItemsSource = searchResults;

            if (!searchResults.Any())
            {
                MessageBox.Show("No lots found containing products with the specified name.",
                    "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadLots();
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Search by Product Name")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Search by Product Name";
                searchTextBox.Foreground = Brushes.Gray;
            }
        }
        private void LoadStockOuts()
        {
            try
            {
                var stockOuts = _stockOutService.GetAllStockOuts();
                StockOutDataGrid.ItemsSource = stockOuts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stock outs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStockOut_Click(object sender, RoutedEventArgs e)
        {
            var addStockOutWindow = new AddStockOutWindow(_stockOutService, _productService, _partnerService);
            addStockOutWindow.Owner = this;
            if (addStockOutWindow.ShowDialog() == true)
            {
                LoadStockOuts(); // Refresh the stock outs list
            }
        }

        private void EditStockOut_Click(object sender, RoutedEventArgs e)
        {
            if (StockOutDataGrid.SelectedItem is StockOut selectedStockOut)
            {
                var editStockOutWindow = new EditStockOutWindow(_stockOutService, _productService, _partnerService, selectedStockOut.StockOutId);
                editStockOutWindow.Owner = this;
                if (editStockOutWindow.ShowDialog() == true)
                {
                    LoadStockOuts(); // Refresh the stock outs list
                }
            }
            else
            {
                MessageBox.Show("Please select a stock out to edit.", "No Stock Out Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ViewStockOutDetails_Click(object sender, RoutedEventArgs e)
        {
            if (StockOutDataGrid.SelectedItem is StockOut selectedStockOut)
            {
                var stockOutDetailsWindow = new StockOutDetailsWindow(_stockOutService, selectedStockOut.StockOutId);
                stockOutDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a stock out to view details.", "No Stock Out Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void ShowAllStockOut_Click(object sender, RoutedEventArgs e)
        {
            LoadStockOuts();
        }

        private void RemoveStockOutPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (stockOutSearchTextBox.Text == "Search by Stock Out ID")
            {
                stockOutSearchTextBox.Text = "";
                stockOutSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddStockOutPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stockOutSearchTextBox.Text))
            {
                stockOutSearchTextBox.Text = "Search by Stock Out ID";
                stockOutSearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void SearchStockOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowAllStorageAreas_Click(object sender, RoutedEventArgs e)
        {
            LoadStorageAreas();
        }


        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(_categoryService);
            addCategoryWindow.Owner = this;

            if (addCategoryWindow.ShowDialog() == true)
            {
                LoadCategories(); // Refresh the categories list
            }
        }


        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a category is selected
            if (CategoryDataGrid.SelectedItem is Category selectedCategory)
            {
                // Open the EditCategoryWindow, passing the selected category's details
                var editCategoryWindow = new EditCategoryWindow(_categoryService, selectedCategory.CategoryId, selectedCategory.Name);
                editCategoryWindow.Owner = this;

                // Show the edit window as a modal dialog
                if (editCategoryWindow.ShowDialog() == true)
                {
                    LoadCategories(); // Refresh the categories list after editing
                }
            }
            else
            {
                MessageBox.Show("Please select a category to edit.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void ShowAllCategories_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        // MainWindow.xaml.cs
        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDataGrid.SelectedItem is Category selectedCategory)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the category '{selectedCategory.Name}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _categoryService.DeleteCategory(selectedCategory.CategoryId);
                        LoadCategories(); // Refresh the categories list
                        MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void categorySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LoadStorageAreas()
        {
            try
            {
                var storageAreas = _storageAreaService.GetAllStorageAreas();
                StorageAreaDataGrid.ItemsSource = storageAreas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading storage areas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchStorageArea_Click(object sender, RoutedEventArgs e)
        {
            string searchText = storageAreaSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "Search by Area Name")
            {
                MessageBox.Show("Please enter an area name to search.", "Search Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var searchResults = _storageAreaService.GetAllStorageAreas()
                                    .Where(area => area.AreaName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                                    .ToList();

            StorageAreaDataGrid.ItemsSource = searchResults;

            if (!searchResults.Any())
            {
                MessageBox.Show("No storage areas found with the specified name.",
                    "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void RemoveStorageAreaPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (storageAreaSearchTextBox.Text == "Search by Area Name")
            {
                storageAreaSearchTextBox.Text = "";
                storageAreaSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddStorageAreaPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(storageAreaSearchTextBox.Text))
            {
                storageAreaSearchTextBox.Text = "Search by Area Name";
                storageAreaSearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void EditStorageArea_Click(object sender, RoutedEventArgs e)
        {
            if (StorageAreaDataGrid.SelectedItem is StorageArea selectedStorageArea)
            {
                var editStorageAreaWindow = new EditStorageAreaWindow(_storageAreaService, selectedStorageArea);
                editStorageAreaWindow.Owner = this;
                if (editStorageAreaWindow.ShowDialog() == true)
                {
                    LoadStorageAreas(); // Refresh storage areas after editing
                }
            }
            else
            {
                MessageBox.Show("Please select a storage area to edit.", "No Storage Area Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddStorageArea_Click(object sender, RoutedEventArgs e)
        {
            var addStorageAreaWindow = new AddStorageAreaWindow(_storageAreaService);
            addStorageAreaWindow.Owner = this;
            if (addStorageAreaWindow.ShowDialog() == true)
            {
                LoadStorageAreas(); // Refresh storage areas after adding
            }
        }

        private void BanStorageArea_Click(object sender, RoutedEventArgs e)
        {
            if (StorageAreaDataGrid.SelectedItem is StorageArea selectedStorageArea)
            {
                // Check if the area is already banned
                if (selectedStorageArea.Status == 0)
                {
                    MessageBox.Show("This storage area is already banned.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Show confirmation dialog
                var result = MessageBox.Show("Are you sure you want to ban this storage area?", "Ban Storage Area", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Set the status to 0 (banned) and update in the database
                        selectedStorageArea.Status = 0;
                        _storageAreaService.UpdateStorageArea(selectedStorageArea);
                        MessageBox.Show("Storage area has been banned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadStorageAreas(); // Refresh the storage areas list to reflect the change
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error banning storage area: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a storage area to ban.", "No Storage Area Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UnBanStorageArea_Click(object sender, RoutedEventArgs e)
        {
            if (StorageAreaDataGrid.SelectedItem is StorageArea selectedStorageArea)
            {
                // Check if the area is actually banned
                if (selectedStorageArea.Status != 0)
                {
                    MessageBox.Show("This storage area is not currently banned.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Show confirmation dialog
                var result = MessageBox.Show("Are you sure you want to unban this storage area?", "UnBan Storage Area", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Set the status to 1 (active) and update in the database
                        selectedStorageArea.Status = 1;
                        _storageAreaService.UpdateStorageArea(selectedStorageArea);
                        MessageBox.Show("Storage area has been unbanned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadStorageAreas(); // Refresh the storage areas list to reflect the change
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error unbanning storage area: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a storage area to unban.", "No Storage Area Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                ProductDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchProduct_Click(object sender, RoutedEventArgs e)
        {
            string searchText = productSearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "Search by Product Name")
            {
                MessageBox.Show("Please enter a product name to search.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var searchResults = _productService.GetAllProducts()
                .Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            ProductDataGrid.ItemsSource = searchResults;

            if (!searchResults.Any())
            {
                MessageBox.Show("No products found with the specified name.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowAllProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow(_productService, _categoryService, _storageAreaService);
            addProductWindow.Owner = this;
            if (addProductWindow.ShowDialog() == true)
            {
                LoadProducts(); // Implement LoadProducts to refresh the product list
            }
        }


        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                var editProductWindow = new EditProductWindow(_productService, _categoryService, _storageAreaService, selectedProduct.ProductId);
                if (editProductWindow.ShowDialog() == true)
                {
                    // Làm mới bảng dữ liệu hoặc lấy lại danh sách sản phẩm nếu cần thiết
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveProductPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (productSearchTextBox.Text == "Search by Product Name")
            {
                productSearchTextBox.Text = "";
                productSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddProductPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productSearchTextBox.Text))
            {
                productSearchTextBox.Text = "Search by Product Name";
                productSearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void DetailsProduct_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có sản phẩm nào được chọn trong danh sách không
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                // Tạo cửa sổ chi tiết sản phẩm và truyền vào ID sản phẩm
                var productDetailsWindow = new ProductDetailsWindow(_productService, selectedProduct.ProductId);

                // Hiển thị cửa sổ chi tiết sản phẩm
                productDetailsWindow.ShowDialog();
            }
            else
            {
                // Hiển thị thông báo nếu không có sản phẩm nào được chọn
                MessageBox.Show("Please select a product to view details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}