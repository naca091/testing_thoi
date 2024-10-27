using BusinessLogic;
using DataAccess.Models;
using System;
using System.Windows;

namespace WarehouseManagementApp
{
    public partial class LotDetailsWindow : Window
    {
        private readonly LotService _lotService;

        public LotDetailsWindow(LotService lotService, int lotId)
        {
            InitializeComponent();
            _lotService = lotService;
            LoadLotDetails(lotId);
        }

        private void LoadLotDetails(int lotId)
        {
            var lot = _lotService.GetLotDetails(lotId);
            if (lot != null)
            {
                LotCodeTextBlock.Text = $"Lot Code: {lot.LotCode}";
                DateInTextBlock.Text = $"Date In: {lot.DateIn:d}";
                PartnerTextBlock.Text = $"Partner: {lot.Partner.Name}";
                NoteTextBlock.Text = $"Note: {lot.Note}";

                // Add this debug code temporarily to check the data
                foreach (var detail in lot.LotDetails)
                {
                    if (detail.Product == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Product is null for LotDetail ID: {detail.LotDetailId}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Product Name: {detail.Product.Name}");
                    }
                }

                LotDetailsDataGrid.ItemsSource = lot.LotDetails;
            }
        }
    }
}