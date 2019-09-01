using API.Controllers;
using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services;
using System;
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
        #endregion

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
                var jsonResult = await controller.Deposit(new DepositRequest() { IBAN = "NL92ABNA8946051078", DepositMoney = 1000}) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.ACCOUNT_NOT_FOUND, value.Error);
            }
        }

    }
}
