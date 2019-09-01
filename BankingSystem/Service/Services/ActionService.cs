using Entity.DBModels;
using Entity.Models;
using Service.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entity.Models.ActionModels;

namespace Service.Services
{
    public class ActionService : BaseService, IActionService
    {

        private readonly IAccountService _accountService;

        public ActionService(BankingSystemContext context, IAccountService accountService) : base(context)
        {
            _accountService = accountService;
        }

        public async Task<DepositResponse> Deposit(string iban, decimal depositMoney, string remark = null)
        {
            if (string.IsNullOrEmpty(iban)) throw new ArgumentException(Entity.Constant.IBAN_IS_NULL);
            if (depositMoney <= 0) throw new ArgumentException(Entity.Constant.DEPOSIT_ZERO_MONEY);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // find account by IBAN number
                    var account = await _accountService.GetAccount(iban);
                    if (account == null) throw new ArgumentException(Entity.Constant.ACCOUNT_NOT_FOUND);
                    // fee calculated
                    decimal fee = (depositMoney * Entity.Constant.Fee_Percent) / 100;
                    // deduct fee charged
                    decimal deductedDepositMoney = depositMoney - fee;
                    // update total amount in account
                    account.TotalAmount += deductedDepositMoney;
                    _context.Entry(account).State = EntityState.Modified;
                    // create new transaction as deposit process
                    var depositTransaction = new Transaction();
                    depositTransaction.Deposit(account, depositMoney, fee, remark);
                    _context.Transactions.Add(depositTransaction);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new DepositResponse(depositTransaction, account);
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
