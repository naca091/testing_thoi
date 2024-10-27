using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class LotDAO
    {

        public void AddLotWithDetails(Lot lot, List<LotDetail> lotDetails)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Lots.Add(lot);
                    _context.SaveChanges();

                    foreach (var detail in lotDetails)
                    {
                        detail.LotId = lot.LotId;
                        _context.LotDetails.Add(detail);
                    }
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateException dbEx)
                {
                    transaction.Rollback();
                    var message = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                    throw new Exception($"An error occurred while adding the lot with details: {message}");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while adding the lot with details.", ex);
                }
            }
        }



        public List<Lot> SearchLotsByProductName(string productName)
        {
            return _context.Lots
                .Include(l => l.Account)
                .Include(l => l.Partner)
                .Include(l => l.LotDetails)
                    .ThenInclude(ld => ld.Product)
                .Where(l => l.LotDetails.Any(ld =>
                    ld.Product.Name.Contains(productName)))
                .ToList();
        }


        private readonly PRN221_Warehouse _context;

        public LotDAO(PRN221_Warehouse context)
        {
            _context = context;
        }


        public void AddLot(Lot lot)
        {
            _context.Lots.Add(lot);
            _context.SaveChanges();
        }

        public Lot GetLotById(int lotId)
        {
            return _context.Lots
                .Include(l => l.Account)
                .Include(l => l.Partner)
                .Include(l => l.LotDetails)
                    .ThenInclude(ld => ld.Product)
                .FirstOrDefault(l => l.LotId == lotId);
        }

        public List<Lot> GetAllLots()
        {
            return _context.Lots
                .Include(l => l.Account)
                .Include(l => l.Partner)
                .ToList();
        }





        public List<Lot> GetLotsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Lots
                .Where(l => l.DateIn >= startDate && l.DateIn <= endDate)
                .Include(l => l.Account)
                .Include(l => l.Partner)
                .ToList();
        }

        public List<Lot> GetLotsByPartner(int partnerId)
        {
            return _context.Lots
                .Where(l => l.PartnerId == partnerId)
                .Include(l => l.Account)
                .Include(l => l.Partner)
                .ToList();
        }

        public void AddLotDetail(LotDetail lotDetail)
        {
            _context.LotDetails.Add(lotDetail);
            _context.SaveChanges();
        }

        public void UpdateLotDetail(LotDetail lotDetail)
        {
            _context.Entry(lotDetail).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteLotDetail(int lotDetailId)
        {
            var lotDetail = _context.LotDetails.Find(lotDetailId);
            if (lotDetail != null)
            {
                _context.LotDetails.Remove(lotDetail);
                _context.SaveChanges();
            }
        }

        public List<LotDetail> GetLotDetailsByLotId(int lotId)
        {
            return _context.LotDetails
                .Where(ld => ld.LotId == lotId)
                .Include(ld => ld.Product)
                .ToList();
        }
        public void UpdateLot(Lot lot, List<LotDetail> updatedLotDetails)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingLot = _context.Lots
                        .Include(l => l.LotDetails)
                        .FirstOrDefault(l => l.LotId == lot.LotId);

                    if (existingLot == null)
                    {
                        throw new Exception("Lot not found.");
                    }

                    // Update lot properties
                    existingLot.LotCode = lot.LotCode;
                    existingLot.PartnerId = lot.PartnerId;
                    existingLot.DateIn = lot.DateIn;
                    existingLot.Note = lot.Note;

                    // Remove existing lot details
                    _context.LotDetails.RemoveRange(existingLot.LotDetails);

                    // Add updated lot details
                    foreach (var detail in updatedLotDetails)
                    {
                        existingLot.LotDetails.Add(detail);
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
        }
    }
}