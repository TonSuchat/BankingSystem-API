using API.Controllers;
using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Entity.Models.ActionModels;

namespace UnitTest.Controllers
{
    public class ActionControllerTest : BaseUnitTest
    {

        #region Seed-Methods
        private void Seed_Deposit_Success(BankingSystemContext context, string iban)
        {
            // add account
            Account account = new Account() { IBAN = iban, CustomerId = 1, IsActive = true, CreatedOn = DateTime.Now, TotalAmount = 500 };
            context.Accounts.Add(account);
            context.SaveChanges();
        }

        private void Seed_Transfer_Success(BankingSystemContext context, string transferIBAN, string receiveIBAN, bool skipTransfer = false, bool skipReceive = false)
        {
            // customers
            List<Customer> customers = new List<Customer>();
            if (!skipTransfer) customers.Add(new Customer() { Id = 1, FirstName = "John", LastName = "Wick", IsActive = true, CreatedOn = DateTime.Now });
            if (!skipReceive) customers.Add(new Customer() { Id = 2, FirstName = "Tony", LastName = "Stark", IsActive = true, CreatedOn = DateTime.Now });
            context.Customers.AddRange(customers);
            context.SaveChanges();

            // add accounts
            List<Account> accounts = new List<Account>();
            if (!skipTransfer) accounts.Add(new Account() { IBAN = transferIBAN, CustomerId = 1, IsActive = true, CreatedOn = DateTime.Now, TotalAmount = 3000 });
            if (!skipReceive) accounts.Add(new Account() { IBAN = receiveIBAN, CustomerId = 2, IsActive = true, CreatedOn = DateTime.Now, TotalAmount = 1000 });
            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }
        #endregion

        #region Deposit
        [Fact]
        public async Task Deposit_Success()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Deposit_Success");
            var iban = "NL92ABNA8946051078";
            var remark = "Put 1000 money into account.";
            decimal depositMoney = 1000;
            using (var context = new BankingSystemContext(options))
            {
                SeedIBANMasterData(context);
                Seed_Deposit_Success(context, iban);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Deposit(new DepositRequest() { IBAN = iban, DepositMoney = depositMoney, Remark = remark }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(200, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.NotNull(value.Result);
                // assert response object
                var depositResponse = value.Result as DepositResponse;
                Assert.NotNull(depositResponse);
                Assert.NotEqual(0, depositResponse.TransactionId);
                Assert.Equal(depositMoney, depositResponse.Deposit);
                Assert.Equal(1, depositResponse.Fee);
                Assert.Equal(1499, depositResponse.TotalAmount);
                // assert account in db
                var account = await context.Accounts.FirstAsync(a => a.IBAN == iban);
                Assert.Equal(1499, account.TotalAmount);
                // assert transaction in db
                var transaction = await context.Transactions.FirstAsync(t => t.Id == depositResponse.TransactionId);
                Assert.Equal(Entity.Enums.TransactionType.DEPOSIT, transaction.Type);
                Assert.Equal(iban, transaction.ReceiveIBAN);
                Assert.Equal(depositMoney, transaction.Amount);
                Assert.Equal(1, transaction.Fee);
                Assert.Equal(remark, transaction.Remark);
            }
        }

        [Fact]
        public async Task Deposit_Zero_Money_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Deposit_Null_IBAN_Error");
            using (var context = new BankingSystemContext(options))
            {
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Deposit(new DepositRequest() { IBAN = "NL92ABNA8946051078", DepositMoney = 0 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.DEPOSIT_ZERO_MONEY, value.Error);
            }
        }

        [Fact]
        public async Task Deposit_Null_IBAN_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Deposit_Null_IBAN_Error");
            using (var context = new BankingSystemContext(options))
            {
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Deposit(new DepositRequest() { IBAN = null, DepositMoney = 1000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.IBAN_IS_NULL, value.Error);
            }
        }

        [Fact]
        public async Task Deposit_Account_Not_Found_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Deposit_Account_Not_Found_Error");
            using (var context = new BankingSystemContext(options))
            {
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Deposit(new DepositRequest() { IBAN = "NL92ABNA8946051078", DepositMoney = 1000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.ACCOUNT_NOT_FOUND, value.Error);
            }
        }
        #endregion

        #region Transfer
        [Fact]
        public async Task Transfer_Success()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_Success");
            var transferIBAN = "NL92ABNA8946051078";
            var receiveIBAN = "NL05INGB6289099205";
            var remark = "Transfer 2000 money to Tony account.";
            decimal transferMoney = 2000;
            using (var context = new BankingSystemContext(options))
            {
                SeedIBANMasterData(context);
                Seed_Transfer_Success(context, transferIBAN, receiveIBAN);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = transferIBAN, ToIBAN = receiveIBAN, Amount = transferMoney, Remark = remark }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(200, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.NotNull(value.Result);
                // assert response object
                var transferResponse = value.Result as TransferResponse;
                Assert.NotNull(transferResponse);
                Assert.NotEqual(0, transferResponse.TransactionId);
                Assert.Equal(transferIBAN, transferResponse.TransferIBAN);
                Assert.Equal(receiveIBAN, transferResponse.ReceiveIBAN);
                Assert.Equal("Tony Stark", transferResponse.ReceiveCustomerFullName);
                Assert.Equal(transferMoney, transferResponse.Amount);
                Assert.Equal(0, transferResponse.Fee);
                // assert accounts in db
                var transferAccount = await context.Accounts.FirstAsync(a => a.IBAN == transferIBAN);
                Assert.Equal(1000, transferAccount.TotalAmount);
                var receiveAccount = await context.Accounts.FirstAsync(a => a.IBAN == receiveIBAN);
                Assert.Equal(3000, receiveAccount.TotalAmount);
                // assert transaction in db
                var transaction = await context.Transactions.FirstAsync(t => t.Id == transferResponse.TransactionId);
                Assert.Equal(Entity.Enums.TransactionType.TRANSFER, transaction.Type);
                Assert.Equal(transferIBAN, transaction.TransferIBAN);
                Assert.Equal(receiveIBAN, transaction.ReceiveIBAN);
                Assert.Equal(transferMoney, transaction.Amount);
                Assert.Equal(0, transaction.Fee);
                Assert.Equal(remark, transaction.Remark);
            }
        }

        [Fact]
        public async Task Transfer_IBAN_Is_Null_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_IBAN_Is_Null_Error");
            using (var context = new BankingSystemContext(options))
            {
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = null, ToIBAN = null, Amount = 20000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.IBAN_IS_NULL, value.Error);
            }
        }

        [Fact]
        public async Task Transfer_Zero_Money_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_Zero_Money_Error");
            var transferIBAN = "NL92ABNA8946051078";
            var receiveIBAN = "NL05INGB6289099205";
            using (var context = new BankingSystemContext(options))
            {
                Seed_Transfer_Success(context, transferIBAN, receiveIBAN);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = transferIBAN, ToIBAN = receiveIBAN, Amount = 0 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.TRANSFER_ZERO_MONEY, value.Error);
            }
        }

        [Fact]
        public async Task Transfer_Not_Found_Trasnfer_Account_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_IBAN_Is_Null_Error");
            var transferIBAN = "NL92ABNA8946051078";
            var receiveIBAN = "NL05INGB6289099205";
            using (var context = new BankingSystemContext(options))
            {
                Seed_Transfer_Success(context, transferIBAN, receiveIBAN, true, false);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = transferIBAN, ToIBAN = receiveIBAN, Amount = 20000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.NOT_FOUND_TRANSFER_ACCOUNT, value.Error);
            }
        }

        [Fact]
        public async Task Transfer_Not_Found_Receive_Account_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_Not_Found_Receive_Account_Error");
            var transferIBAN = "NL92ABNA8946051078";
            var receiveIBAN = "NL05INGB6289099205";
            using (var context = new BankingSystemContext(options))
            {
                Seed_Transfer_Success(context, transferIBAN, receiveIBAN, false, true);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = transferIBAN, ToIBAN = receiveIBAN, Amount = 20000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.NOT_FOUND_RECEIVE_ACCOUNT, value.Error);
            }
        }

        [Fact]
        public async Task Transfer_Not_Enough_Money_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Transfer_Not_Enough_Money_Error");
            var transferIBAN = "NL92ABNA8946051078";
            var receiveIBAN = "NL05INGB6289099205";
            using (var context = new BankingSystemContext(options))
            {
                Seed_Transfer_Success(context, transferIBAN, receiveIBAN);
                // ACT
                var accountService = new AccountService(context);
                var actionService = new ActionService(context, accountService);
                var controller = new ActionController(context, accountService, actionService);
                // ACT
                var jsonResult = await controller.Transfer(new TransferReqeuest() { FromIBAN = transferIBAN, ToIBAN = receiveIBAN, Amount = 1000000 }) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.TRANSFER_MONEY_NOT_ENOUGH, value.Error);
            }
        }
        #endregion

    }
}
