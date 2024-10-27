using BusinessLogic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class StockOutDetailsWindow : Window
    {
        private readonly StockOutService _stockOutService;

        public StockOutDetailsWindow(StockOutService stockOutService, int stockOutId)
        {
            InitializeComponent();
            _stockOutService = stockOutService;
            LoadStockOutDetails(stockOutId);
        }

        private void LoadStockOutDetails(int stockOutId)
        {
            var stockOut = _stockOutService.GetStockOutById(stockOutId);
            if (stockOut != null)
            {
                StockOutIdTextBlock.Text = $"Stock Out ID: {stockOut.StockOutId}";
                PartnerTextBlock.Text = $"Partner: {stockOut.Partner.Name}";
                DateOutTextBlock.Text = $"Date Out: {stockOut.DateOut:d}";
                NoteTextBlock.Text = $"Note: {stockOut.Note}";

                StockOutDetailsDataGrid.ItemsSource = stockOut.StockOutDetails;
            }
        }
    }
}
