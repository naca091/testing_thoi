using BusinessLogic;
using DataAccess;
using DataAccess.Models;
using System.Globalization;
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
        private readonly AccountService _accountService;




        public MainWindow()
        {

            InitializeComponent();
            ConfigureTabItemVisibility();
            ConfigureButtonVisibility();
            var context = new PRN221_Warehouse();

            _lotService = new LotService(context);
            _productService = new ProductService(context);
            _partnerService = new PartnerService(context);
            _categoryService = new CategoryService(new CategoryDAO(context)); // Initialize CategoryService

            var stockOutDAO = new StockOutDAO(context);
            _stockOutService = new StockOutService(stockOutDAO);
            _storageAreaService = new StorageAreaService(new StorageAreaDAO(context));
            _accountService = new AccountService(context);
            LoadAccounts();
            LoadPartner();
            LoadStorageAreas();
            LoadProducts();

            LoadLots();
            LoadStockOuts();
            LoadCategories(); // Load categories on startup
        }

        //config tab item Account for Admin
        private void ConfigureTabItemVisibility()
        {
            var loggedInAccount = LoginWindow.LoggedInAccount;

            // Kiểm tra xem TabItem có tồn tại không
            if (AccountsTabItem != null)
            {
                // Chỉ hiển thị TabItem nếu role là 1
                if (loggedInAccount?.Role != 0)
                {
                    // Tìm TabControl chứa TabItem
                    if (AccountsTabItem.Parent is TabControl tabControl)
                    {
                        // Remove TabItem khỏi TabControl
                        tabControl.Items.Remove(AccountsTabItem);
                    }
                }
            }
        }
        //config for see or not see button
        //config for see or not see button
        private void ConfigureButtonVisibility()
        {
            // Get the logged-in account from LoginWindow
            var loggedInAccount = LoginWindow.LoggedInAccount;

            // Check if buttons exist (they might be in a TabItem)
            if (loggedInAccount != null)
            {
                // Role 1 and Role 2 can see AddLot, AddProduct, AddStockOut
                bool canSeeAddLotProductStockOut = loggedInAccount.Role == 1 || loggedInAccount.Role == 2;

                // Only Role 2 can see other buttons
                bool canSeeRole2OnlyButtons = loggedInAccount.Role == 2;

                // Configure visibility for buttons accessible to Role 1 and Role 2
                AddLotButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;
                AddProductButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;
                AddStockOutButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;
                ViewDetailsButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;
                ViewStockOutDetailsButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;
                DetailsProductButton.Visibility = canSeeAddLotProductStockOut ? Visibility.Visible : Visibility.Collapsed;


                // Configure visibility for Role 2-only buttons
                EditLotButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                EditStockOutButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                AddCategoryButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                EditCategoryButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                DeleteCategoryButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                AddStorageAreaButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                EditStorageAreaButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                BanStorageAreaButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                UnBanStorageAreaButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                EditProductButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                BanProductButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                UnBanProductButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                AddPartnerButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                EditPartnerButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                BanPartnerButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;
                UnbanPartnerButton.Visibility = canSeeRole2OnlyButtons ? Visibility.Visible : Visibility.Collapsed;



                // Repeat similar lines for other Role 2-only buttons
            }
        }


        private void LoadAccounts()
            {
                AccountDataGrid.ItemsSource = _accountService.GetAllAccounts();
            }
        

        private void LoadPartner()
        {
            // Fetch all partners from the service
            List<Partner> partners = _partnerService.GetAllPartners();

            // Bind the fetched partners to the DataGrid
            PartnerDataGrid.ItemsSource = partners;
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
            if (StockOutSearchTextBox.Text == "Search by Stock Out ID")
            {
                StockOutSearchTextBox.Text = "";
                StockOutSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddStockOutPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StockOutSearchTextBox.Text))
            {
                StockOutSearchTextBox.Text = "Search by Stock Out ID";
                StockOutSearchTextBox.Foreground = Brushes.Gray;
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
            // Check if a product is selected in the data grid
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                // Initialize the EditProductWindow and pass the selected product's ID
                var editProductWindow = new EditProductWindow(_productService, _categoryService, _storageAreaService, selectedProduct.ProductId)
                {
                    Owner = this // Sets the current window as the owner of the dialog
                };

                // Show dialog and check if the result was 'true' (indicating a save)
                if (editProductWindow.ShowDialog() == true)
                {
                    // Refresh product list or data grid to reflect changes
                    LoadProducts();
                }
            }
            else
            {
                // Show message if no product is selected
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

        private void BanProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                // Show confirmation dialog
                var result = MessageBox.Show("Are you sure you want to Ban this product?", "Confirm Ban", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Update product status to 0 (Ban)
                        selectedProduct.Status = 0;
                        _productService.UpdateProduct(selectedProduct);

                        MessageBox.Show("Product has been banned.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Refresh product list or data grid to reflect changes
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error banning product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to ban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UnBanProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                // Show confirmation dialog
                var result = MessageBox.Show("Are you sure you want to UnBan this product?", "Confirm UnBan", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Update product status to 1 (UnBan)
                        selectedProduct.Status = 1;
                        _productService.UpdateProduct(selectedProduct);

                        MessageBox.Show("Product has been unbanned.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Refresh product list or data grid to reflect changes
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error unbanning product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to unban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SearchPartner_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = partnerSearchTextBox.Text;
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm != "Search by Partner Name")
            {
                PartnerDataGrid.ItemsSource = _partnerService.GetAllPartners()
                    .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        // Show all Partners
        private void ShowAllPartners_Click(object sender, RoutedEventArgs e)
        {
            LoadPartner();
            partnerSearchTextBox.Text = "Search by Partner Name";
            partnerSearchTextBox.Foreground = Brushes.Gray;
        }

        // Placeholder text functionality for search box
        private void RemovePartnerPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (partnerSearchTextBox.Text == "Search by Partner Name")
            {
                partnerSearchTextBox.Text = "";
                partnerSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddPartnerPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(partnerSearchTextBox.Text))
            {
                partnerSearchTextBox.Text = "Search by Partner Name";
                partnerSearchTextBox.Foreground = Brushes.Gray;
            }
        }


        private void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            var addPartnerWindow = new AddPartnerWindow(_partnerService);
            if (addPartnerWindow.ShowDialog() == true)
            {
                LoadPartner(); // Refresh data
            }
        }

        private void EditPartner_Click(object sender, RoutedEventArgs e)
        {
            if (PartnerDataGrid.SelectedItem is Partner selectedPartner)
            {
                var editPartnerWindow = new EditPartnerWindow(_partnerService, selectedPartner);
                if (editPartnerWindow.ShowDialog() == true)
                {
                    LoadPartner(); // Refresh data
                }
            }
            else
            {
                MessageBox.Show("Please select a partner to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BanPartner_Click(object sender, RoutedEventArgs e)
        {
            if (PartnerDataGrid.SelectedItem is Partner selectedPartner)
            {
                var result = MessageBox.Show("Are you sure you want to ban this partner?", "Confirm Ban", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _partnerService.BanPartner(selectedPartner.PartnerId);
                    MessageBox.Show("Partner has been banned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadPartner(); // Refresh partner data
                }
            }
            else
            {
                MessageBox.Show("Please select a partner to ban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UnbanPartner_Click(object sender, RoutedEventArgs e)
        {
            if (PartnerDataGrid.SelectedItem is Partner selectedPartner)
            {
                var result = MessageBox.Show("Are you sure you want to unban this partner?", "Confirm Unban", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _partnerService.UnbanPartner(selectedPartner.PartnerId);
                    MessageBox.Show("Partner has been unbanned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadPartner(); // Refresh partner data
                }
            }
            else
            {
                MessageBox.Show("Please select a partner to unban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void SearchAccount_Click(object sender, RoutedEventArgs e)
        {
            string query = accountSearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(query))
            {
                AccountDataGrid.ItemsSource = _accountService.SearchAccounts(query);
            }
        }

        private void ShowAllAccounts_Click(object sender, RoutedEventArgs e)
        {
            LoadAccounts(); // Reload all accounts
        }

        private void RemoveAccountPlaceholderText(object sender, RoutedEventArgs e)
        {
            // Cast sender to TextBox to access its properties
            if (sender is TextBox textBox)
            {
                // Check if the placeholder text is displayed
                if (textBox.Text == "Search by Email or Name")
                {
                    // Clear the placeholder text and set the text color to black
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void AddAccountPlaceholderText(object sender, RoutedEventArgs e)
        {
            // Cast sender to TextBox to access its properties
            if (sender is TextBox textBox)
            {
                // Check if the text box is empty
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    // Set the placeholder text and set the text color to gray
                    textBox.Text = "Search by Email or Name";
                    textBox.Foreground = Brushes.Gray;
                }
            }
        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            var addAccountWindow = new AddAccountWindow(_accountService);
            if (addAccountWindow.ShowDialog() == true)
            {
                LoadAccounts(); // Refresh accounts after adding
            }
        }
        private void BanAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AccountDataGrid.SelectedItem is Account selectedAccount)
            {
                var result = MessageBox.Show("Are you sure you want to ban this account?", "Confirm Ban", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    selectedAccount.Status = 0; // Set status to 0 for banned
                    _accountService.UpdateAccount(selectedAccount);
                    MessageBox.Show("Account banned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadAccounts();
                }
            }
            else
            {
                MessageBox.Show("Please select an account to ban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UnBanAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AccountDataGrid.SelectedItem is Account selectedAccount)
            {
                var result = MessageBox.Show("Are you sure you want to unban this account?", "Confirm UnBan", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    selectedAccount.Status = 1; // Set status to 1 for active
                    _accountService.UpdateAccount(selectedAccount);
                    MessageBox.Show("Account unbanned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadAccounts();
                }
            }
            else
            {
                MessageBox.Show("Please select an account to unban.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Show a confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Check if the user clicked "Yes"
            if (result == MessageBoxResult.Yes)
            {
                // Show the LoginWindow
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Close the current MainWindow
                this.Close();
            }
            // If "No" is selected, simply close the popup and stay in MainWindow
        }

        // Stock Out Search Event
        private void StockOutSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = StockOutSearchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText) && searchText != "search by partner name")
            {
                // Fetch all StockOuts and their associated Partner names
                var filteredStockOuts = _stockOutService.GetAllStockOuts()
                    .Where(stockOut => stockOut.Partner.Name.ToLower().Contains(searchText))
                    .ToList();

                StockOutDataGrid.ItemsSource = filteredStockOuts;
            }
            else
            {
                // If the search text is empty or is the placeholder text, show all stock outs
                ShowAllStockOut_Click(sender, e);
            }
        }




        // Category Search Event
        private void CategorySearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = CategorySearchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Filter Categories based on the CategoryCode
                var filteredCategories = _categoryService.GetAllCategories()
                                        .Where(category => category.CategoryCode.ToLower().Contains(searchText))
                                        .ToList();
                CategoryDataGrid.ItemsSource = filteredCategories;
            }
        }

        private void ShowAllCategory_Click(object sender, RoutedEventArgs e)
        {
            // Display all Categories
            CategoryDataGrid.ItemsSource = _categoryService.GetAllCategories();
        }
        private void RemoveCategoryPlaceholderText(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Category Code" || textBox.Text == "Name")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black; // Change to your desired foreground color
            }
        }

        private void AddCategoryPlaceholderText(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                if (textBox.Name == "CategoryCodeTextBox") // Check the name of the TextBox
                {
                    textBox.Text = "Category Code"; // Placeholder text
                }
                else if (textBox.Name == "NameTextBox")
                {
                    textBox.Text = "Name"; // Placeholder text for name
                }
                textBox.Foreground = Brushes.Gray; // Change to your desired foreground color for placeholder
            }
        }


    }
}