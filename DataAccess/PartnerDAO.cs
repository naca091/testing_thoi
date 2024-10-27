using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
    }
}