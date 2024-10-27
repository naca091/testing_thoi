using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;
using System.Transactions;

namespace BusinessLogic
{
    public class LotService
    {


        public void AddLotWithDetails(Lot lot, List<LotDetail> lotDetails)
        {
            // Thêm lot và các chi tiết liên quan
            _lotDAO.AddLotWithDetails(lot, lotDetails);

            // Cập nhật số lượng của từng sản phẩm trong LotDetail
            foreach (var detail in lotDetails)
            {
                _productService.UpdateProductQuantity(detail.ProductId, detail.Quantity);
            }
        }

        private readonly LotDAO _lotDAO;
        private readonly ProductService _productService;

        public LotService(PRN221_Warehouse context)
        {
            _lotDAO = new LotDAO(context);
            _productService = new ProductService(context);
        }
        public Lot GetLotById(int lotId)
        {
            return _lotDAO.GetLotById(lotId);
        }
        public void UpdateLot(Lot lot, List<LotDetail> updatedLotDetails)
        {
            _lotDAO.UpdateLot(lot, updatedLotDetails);
        }



        public Lot GetLotDetails(int lotId)
        {
            return _lotDAO.GetLotById(lotId);
        }

        public List<Lot> GetAllLots()
        {
            return _lotDAO.GetAllLots();
        }

        public List<Lot> SearchLotsByProductName(string productName)
        {
            return _lotDAO.SearchLotsByProductName(productName);
        }


    }
}