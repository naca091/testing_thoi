using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class PartnerDAO
    {
        private readonly PRN221_Warehouse _context;

        public PartnerDAO(PRN221_Warehouse context)
        {
            _context = context;
        }

        public List<Partner> GetAllPartners()
        {
            return _context.Partners.ToList();
        }

        public Partner GetPartnerByID(int partnerId)
        {
            return _context.Partners.Find(partnerId);
        }

        public void AddPartner(Partner partner)
        {
            _context.Partners.Add(partner);
            _context.SaveChanges();
        }

        public void UpdatePartner(Partner partner)
        {
            var existingPartner = _context.Partners.Find(partner.PartnerId);
            if (existingPartner != null)
            {
                existingPartner.PartnerCode = partner.PartnerCode;
                existingPartner.Name = partner.Name;
                existingPartner.Status = partner.Status;
                _context.SaveChanges();
            }
        }
        public void BanPartner(int partnerId)
        {
            var partner = _context.Partners.Find(partnerId);
            if (partner != null)
            {
                partner.Status = 0; // Status 0 for banned
                _context.SaveChanges();
            }
        }

        public void UnbanPartner(int partnerId)
        {
            var partner = _context.Partners.Find(partnerId);
            if (partner != null)
            {
                partner.Status = 1; // Status 1 for unbanned
                _context.SaveChanges();
            }
        }
    }
}
