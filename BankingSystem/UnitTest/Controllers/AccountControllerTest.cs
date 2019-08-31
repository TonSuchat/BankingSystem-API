using API.Controllers;
using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;

namespace UnitTest.Controllers
{
    public class AccountControllerTest : BaseUnitTest
    {

        [Fact]
        public async Task Create_An_Account_Success()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Create_An_Account_Success");
            var customer = new Customer() { FirstName = "John", LastName = "Wick" };
            using (var context = new BankingSystemContext(options))
            {
                SeedIBANMasterData(context);
                var accountService = new AccountService(context);
                var controller = new AccountController(context, accountService);
                // ACT
                var jsonResult = await controller.CreateAccount(customer, 500) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(200, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.NotEqual(string.IsNullOrEmpty(""), value.Result);
            }
        }

        [Fact]
        public async Task Create_An_Account_Customer_Null_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Create_An_Account_Customer_Null_Error");
            using (var context = new BankingSystemContext(options))
            {
                SeedIBANMasterData(context);
                var accountService = new AccountService(context);
                var controller = new AccountController(context, accountService);
                // ACT
                var jsonResult = await controller.CreateAccount(null) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(500, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.CREATEACCOUNT_CUSTOMER_IS_NULL, value.Error);
            }
        }

        [Fact]
        public async Task Create_An_Account_No_IBAN_Left_Error()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Create_An_Account_No_IBAN_Left_Error");
            var customer = new Customer() { FirstName = "John", LastName = "Wick" };
            using (var context = new BankingSystemContext(options))
            {
                var accountService = new AccountService(context);
                var controller = new AccountController(context, accountService);
                // ACT
                var jsonResult = await controller.CreateAccount(customer, 500) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(500, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.CREATEACCOUNT_NO_IBAN_LEFT, value.Error);
            }
        }

    }
}
