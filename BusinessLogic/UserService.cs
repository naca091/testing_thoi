
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class UserService
    {
        private readonly PRN221_Warehouse _context;

        public UserService()
        {
            _context = new PRN221_Warehouse();
        }

        public bool Authenticate(string email, string password)
        {
            return _context.Accounts.Any(a => a.Email == email && a.Password == password && a.Role == 2);
        }
    }
}
