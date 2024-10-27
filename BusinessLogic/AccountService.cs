using DataAccess;
using DataAccess.Models;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class AccountService
    {
        private readonly AccountDAO _accountDAO;

        public AccountService(PRN221_Warehouse context)
        {
            _accountDAO = new AccountDAO(context);
        }

        public List<Account> GetAllAccounts()
        {
            return _accountDAO.GetAllAccounts();
        }

        public Account GetAccountById(int accountId)
        {
            return _accountDAO.GetAccountById(accountId);
        }

        public void AddAccount(Account account)
        {
            _accountDAO.AddAccount(account);
        }

        public void UpdateAccount(Account account)
        {
            _accountDAO.UpdateAccount(account);
        }

        public List<Account> SearchAccounts(string query)
        {
            return _accountDAO.SearchAccounts(query);
        }

        //code cho login
        public Account? GetAccountByEmailAndPassword(string email, string password)
        {
            return _accountDAO.GetAllAccounts()
                              .FirstOrDefault(a => a.Email == email && a.Password == password);
        }

    }
}
