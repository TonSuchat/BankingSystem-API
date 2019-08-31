using Entity.DBModels;
using Logger;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Service.Services
{
    public class AccountService : IAccountService
    {

        private readonly BankingSystemContext _context;

        public AccountService(BankingSystemContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAccount(Customer customer, decimal initialMoney = 0)
        {
            if (customer == null) throw new ArgumentException(Entity.Constant.CREATEACCOUNT_CUSTOMER_IS_NULL);
            var masterIBAN = await _context.MasterIBANs.FirstOrDefaultAsync(m => !m.Used);
            if (masterIBAN == null) throw new Exception(Entity.Constant.CREATEACCOUNT_NO_IBAN_LEFT);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // add customer
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    // add account
                    var account = new Account() { CustomerId = customer.Id, IBAN = masterIBAN.IBAN, TotalAmount = initialMoney };
                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return account.IBAN;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    
    }
}
