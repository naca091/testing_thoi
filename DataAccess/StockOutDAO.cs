using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StockOutDAO
    {
        private readonly PRN221_Warehouse _context;

        public StockOutDAO(PRN221_Warehouse context)
        {
            _context = context;
        }



        public List<StockOut> GetAllStockOuts()
        {
            return _context.StockOuts
                .Include(s => s.Partner)
                .Include(s => s.Account)
                .ToList();
        }


        public StockOut GetStockOutById(int stockOutId)
        {
            return _context.StockOuts
                .Include(s => s.StockOutDetails)
                    .ThenInclude(d => d.Product)
                .Include(s => s.Partner)
                .FirstOrDefault(s => s.StockOutId == stockOutId);
        }

        public void AddStockOutWithDetails(StockOut stockOut, List<StockOutDetail> details)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Add the stock out record
                _context.StockOuts.Add(stockOut);
                _context.SaveChanges();

                // Get the generated StockOutId
                int stockOutId = stockOut.StockOutId;

                foreach (var detail in details)
                {
                    // Set the StockOutId for each detail
                    detail.StockOutId = stockOutId;

                    // Get the product and check if there's enough quantity
                    var product = _context.Products.Find(detail.ProductId);
                    if (product == null || product.Quantity < detail.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient quantity for product {product?.Name ?? "Unknown"}. " +
                            $"Available: {product?.Quantity ?? 0}, Requested: {detail.Quantity}");
                    }

                    // Decrease the product quantity
                    product.Quantity -= detail.Quantity;

                    // Add the detail record
                    _context.StockOutDetails.Add(detail);
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdateStockOut(StockOut stockOut)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingStockOut = _context.StockOuts
                        .Include(s => s.StockOutDetails)
                        .FirstOrDefault(s => s.StockOutId == stockOut.StockOutId);

                    if (existingStockOut == null)
                        throw new Exception("StockOut not found.");

                    // Update StockOut main fields
                    existingStockOut.PartnerId = stockOut.PartnerId;
                    existingStockOut.DateOut = stockOut.DateOut;
                    existingStockOut.Note = stockOut.Note;

                    // Update StockOutDetails
                    foreach (var detail in stockOut.StockOutDetails)
                    {
                        var existingDetail = existingStockOut.StockOutDetails
                            .FirstOrDefault(d => d.StockOutDetailId == detail.StockOutDetailId);

                        if (existingDetail != null)
                        {
                            existingDetail.Quantity = detail.Quantity;
                            existingDetail.ProductId = detail.ProductId;
                        }
                        else
                        {
                            // Add new detail
                            existingStockOut.StockOutDetails.Add(detail);
                        }
                    }

                    // Remove details that are no longer in the list
                    var detailsToRemove = existingStockOut.StockOutDetails
                        .Where(d => !stockOut.StockOutDetails.Any(nd => nd.StockOutDetailId == d.StockOutDetailId))
                        .ToList();

                    _context.StockOutDetails.RemoveRange(detailsToRemove);

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void DeleteStockOut(int stockOutId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var stockOut = _context.StockOuts
                    .Include(s => s.StockOutDetails)
                    .FirstOrDefault(s => s.StockOutId == stockOutId);

                if (stockOut != null)
                {
                    // Restore product quantities before deleting
                    foreach (var detail in stockOut.StockOutDetails)
                    {
                        var product = _context.Products.Find(detail.ProductId);
                        if (product != null)
                        {
                            product.Quantity += detail.Quantity;
                        }
                    }

                    _context.StockOuts.Remove(stockOut);
                    _context.SaveChanges();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<StockOut> GetStockOutsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.StockOuts
                .Include(s => s.Partner)
                .Include(s => s.StockOutDetails)
                    .ThenInclude(d => d.Product)
                .Where(s => s.DateOut >= startDate && s.DateOut <= endDate)
                .OrderByDescending(s => s.DateOut)
                .ToList();
        }

        public List<StockOut> GetStockOutsByPartnerId(int partnerId)
        {
            return _context.StockOuts
                .Include(s => s.Partner)
                .Include(s => s.StockOutDetails)
                    .ThenInclude(d => d.Product)
                .Where(s => s.PartnerId == partnerId)
                .OrderByDescending(s => s.DateOut)
                .ToList();
        }
    }
}
