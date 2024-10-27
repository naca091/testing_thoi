using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WarehouseManagementApp
{
    /// <summary>
    /// Interaction logic for EditCategoryWindow.xaml
    /// </summary>
    public partial class EditCategoryWindow : Window
    {
        private readonly CategoryService _categoryService;
        private readonly int _categoryId;

        // Constructor with 3 parameters: CategoryService, categoryId, and categoryName
        public EditCategoryWindow(CategoryService categoryService, int categoryId, string categoryName)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _categoryId = categoryId;

            CategoryCodeTextBox.Text = categoryId.ToString(); // Display ID or Code if desired
            CategoryNameTextBox.Text = categoryName;          // Set initial name in the text box
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newName = CategoryNameTextBox.Text;
                _categoryService.UpdateCategoryName(_categoryId, newName);
                MessageBox.Show("Category name updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; // Close the edit window and indicate success
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
