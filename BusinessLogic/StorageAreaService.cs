using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class StorageAreaService
    {
        private readonly StorageAreaDAO _storageAreaDAO;

        public StorageAreaService(StorageAreaDAO storageAreaDAO)
        {
            _storageAreaDAO = storageAreaDAO;
        }

        public IEnumerable<StorageArea> GetAllStorageAreas()
        {
            return _storageAreaDAO.GetAllStorageAreas();
        }

        public void AddStorageArea(StorageArea storageArea)
        {
            if (_storageAreaDAO.IsAreaCodeExists(storageArea.AreaCode))
            {
                throw new Exception("Area Code already exists.");
            }
            _storageAreaDAO.AddStorageArea(storageArea);
        }
        public void UpdateStorageArea(StorageArea storageArea)
        {
            if (storageArea == null)
                throw new ArgumentNullException(nameof(storageArea));

            _storageAreaDAO.UpdateStorageArea(storageArea);
        }

    }
}
