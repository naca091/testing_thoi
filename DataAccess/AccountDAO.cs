using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class AccountDAO
    {
        private readonly PRN221_Warehouse _context;

        public AccountDAO(PRN221_Warehouse context)
        {
            _context = context;
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account GetAccountById(int accountId)
        {
            return _context.Accounts.Find(accountId);
        }

        public void AddAccount(Account account)
        {

                _context.Accounts.Add(account);
                _context.SaveChanges();
            
        }


        public void UpdateAccount(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public List<Account> SearchAccounts(string query)
        {
            return _context.Accounts
                .Where(a => a.Email.Contains(query) || a.Name.Contains(query))
                .ToList();
        }





    }
}
