using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StorageAreaDAO
    {
        private readonly PRN221_Warehouse _context;

        public StorageAreaDAO(PRN221_Warehouse context)
        {
            _context = context;
        }

        public IEnumerable<StorageArea> GetAllStorageAreas()
        {
            return _context.StorageAreas.ToList();
        }
        public bool IsAreaCodeExists(string areaCode)
        {
            return _context.StorageAreas.Any(a => a.AreaCode == areaCode);
        }

        public void AddStorageArea(StorageArea storageArea)
        {
            _context.StorageAreas.Add(storageArea);
            _context.SaveChanges();
        }
        public void UpdateStorageArea(StorageArea storageArea)
        {
            var existingArea = _context.StorageAreas.Find(storageArea.AreaId);
            if (existingArea != null)
            {
                existingArea.AreaName = storageArea.AreaName;
                existingArea.Status = storageArea.Status; // Update the status to banned
                _context.SaveChanges();
            }
        }


    }
}
