using Entity.DBModels;
using Service.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
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

        public async Task<TransferResponse> Transfer(string fromIBAN, string toIBAN, decimal amount, string remark = null)
        {
            if (string.IsNullOrEmpty(fromIBAN) || string.IsNullOrEmpty(toIBAN)) throw new ArgumentException(Entity.Constant.IBAN_IS_NULL);
            if (amount <= 0) throw new ArgumentException(Entity.Constant.TRANSFER_ZERO_MONEY);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // find transfer account
                    var transferAccount = await _accountService.GetAccount(fromIBAN);
                    if (transferAccount == null) throw new ArgumentException(Entity.Constant.NOT_FOUND_TRANSFER_ACCOUNT);
                    // find receive account
                    var receiveAccount = await _accountService.GetAccount(toIBAN, true);
                    if (receiveAccount == null) throw new ArgumentException(Entity.Constant.NOT_FOUND_RECEIVE_ACCOUNT);
                    // check transfer money is enough
                    if (transferAccount.TotalAmount < amount) throw new ArgumentException(Entity.Constant.TRANSFER_MONEY_NOT_ENOUGH);
                    // deduct money from transfer account
                    transferAccount.TotalAmount -= amount;
                    // add money into receive account
                    receiveAccount.TotalAmount += amount;
                    _context.Entry(transferAccount).State = EntityState.Modified;
                    _context.Entry(receiveAccount).State = EntityState.Modified;
                    // add new transaction
                    var transferTransaction = new Transaction();
                    transferTransaction.Transfer(fromIBAN, toIBAN, amount, remark);
                    _context.Transactions.Add(transferTransaction);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new TransferResponse(transferTransaction, transferAccount, receiveAccount);
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
