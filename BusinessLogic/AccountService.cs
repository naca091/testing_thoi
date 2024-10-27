using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AccountService
    {
        private readonly AccountDAO _accountDAO;

        public AccountService()
        {
            _accountDAO = new AccountDAO();
        }

        public Account GetAccountByID(int id)
        {
            return _accountDAO.GetAccountByID(id);
        }
    }
}
