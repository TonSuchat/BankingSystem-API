using Entity.DBModels;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using static Entity.Models.AccountModels;

namespace Service.Services
{
    public class AccountService : IAccountService
    {

        private readonly BankingSystemContext _context;

        public AccountService(BankingSystemContext context)
        {
            _context = context;
        }

        public async Task<CreateAccountResponse> CreateAccount(Customer customer, decimal initialMoney = 0)
        {
            if (customer == null) throw new ArgumentException(Entity.Constant.CREATEACCOUNT_CUSTOMER_IS_NULL);
            var masterIBAN = await _context.MasterIBANs.FirstOrDefaultAsync(m => !m.Used);
            if (masterIBAN == null) throw new Exception(Entity.Constant.CREATEACCOUNT_NO_IBAN_LEFT);
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

    }
}
