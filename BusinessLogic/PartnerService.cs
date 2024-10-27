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
            return _partnerDAO.GetAllPartners();
        }

        public Partner GetPartnerByID(int partnerId)
        {
            return _partnerDAO.GetPartnerByID(partnerId);
        }

        public void AddPartner(Partner partner)
        {
            _partnerDAO.AddPartner(partner);
        }

        public void UpdatePartner(Partner partner)
        {
            _partnerDAO.UpdatePartner(partner);
        }
        public void BanPartner(int partnerId)
        {
            _partnerDAO.BanPartner(partnerId);
        }

        public void UnbanPartner(int partnerId)
        {
            _partnerDAO.UnbanPartner(partnerId);
        }
    }
}
