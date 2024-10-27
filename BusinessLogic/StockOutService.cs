using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class StockOutService
    {
        private readonly StockOutDAO _stockOutDAO;

        public StockOutService(StockOutDAO stockOutDAO)
        {
            _stockOutDAO = stockOutDAO;
        }


        public List<StockOut> GetAllStockOuts()
        {
            return _stockOutDAO.GetAllStockOuts();
        }

        public StockOut GetStockOutById(int stockOutId)
        {
            return _stockOutDAO.GetStockOutById(stockOutId);
        }

        public void AddStockOutWithDetails(StockOut stockOut, List<StockOutDetail> details)
        {
            // Additional business logic validation can be added here
            if (stockOut == null)
                throw new ArgumentNullException(nameof(stockOut));

            if (details == null || details.Count == 0)
                throw new ArgumentException("Stock out details cannot be empty");

            _stockOutDAO.AddStockOutWithDetails(stockOut, details);
        }
        public void UpdateStockOut(StockOut stockOut)
        {
            try
            {
                _stockOutDAO.UpdateStockOut(stockOut);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Stock Out", ex);
            }
        }

        public void DeleteStockOut(int stockOutId)
        {
            _stockOutDAO.DeleteStockOut(stockOutId);
        }

        public List<StockOut> GetStockOutsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _stockOutDAO.GetStockOutsByDateRange(startDate, endDate);
        }

        public List<StockOut> GetStockOutsByPartnerId(int partnerId)
        {
            return _stockOutDAO.GetStockOutsByPartnerId(partnerId);
        }
    }
}   
