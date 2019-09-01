using Entity.DBModels;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Entity.Models.AccountModels;
using System.Linq;

namespace Service.Services
{
    public class AccountService : BaseService, IAccountService
    {

        public AccountService(BankingSystemContext context) : base(context) { }

        public async Task<CreateAccountResponse> CreateAccount(Customer customer, decimal initialMoney = 0)
        {
            if (customer == null || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.LastName)) throw new ArgumentException(Entity.Constant.CUSTOMER_IS_NULL);
            var masterIBAN = await _context.MasterIBANs.FirstOrDefaultAsync(m => !m.Used);
            if (masterIBAN == null) throw new Exception(Entity.Constant.NO_IBAN_LEFT);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // check if customer is existing
                    var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.FirstName == customer.FirstName && c.LastName == customer.LastName);
                    if (existingCustomer == null)
                    {
                        // add customer
                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }
                    else customer = existingCustomer;
                    // add account
                    var account = new Account() { CustomerId = customer.Id, IBAN = masterIBAN.IBAN, TotalAmount = initialMoney };
                    _context.Accounts.Add(account);
                    // flag used account number
                    masterIBAN.Used = true;
                    _context.Entry(masterIBAN).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new CreateAccountResponse(account, customer);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<Account> GetAccount(string iban)
        {
            if (string.IsNullOrEmpty(iban)) throw new ArgumentException(Entity.Constant.IBAN_IS_NULL);
            return await _context.Accounts.FirstOrDefaultAsync(a => a.IBAN == iban && a.IsActive);
        }

        public async Task<IList<Account>> GetAccounts()
        {
            return await _context.Accounts.Where(a => a.IsActive).ToListAsync();
        }
    }
}
