using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class PartnerService
    {
        private readonly PartnerDAO _partnerDAO;

        public PartnerService(PRN221_Warehouse context)
        {
            _partnerDAO = new PartnerDAO(context);
        }

        public List<Partner> GetAllPartners()
        {
            var partners = _partnerDAO.GetAllPartners();
            return partners ?? new List<Partner>();
        }

        public Partner GetPartnerByID(int partnerId)
        {
            return _partnerDAO.GetPartnerByID(partnerId);
        }

    }
}