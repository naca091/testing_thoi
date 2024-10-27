using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AccountDAO
    {
        private readonly PRN221_Warehouse _context;

        public AccountDAO()
        {
            _context = new PRN221_Warehouse();
        }

        public Account GetAccountByID(int id)
        {
            return _context.Accounts.Find(id);
        }
    }
}
