using API.Controllers;
using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using static Entity.Models.AccountModels;

namespace UnitTest.Controllers
{
    public class AccountControllerTest : BaseUnitTest
    {

        #region Seed-Methods
        private void Seed_Create_An_Account_Existing_Customer_Success(BankingSystemContext context, Customer customer)
        {
            // create customer
            context.Customers.Add(customer);
            context.SaveChanges();
        }
        #endregion

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
                Assert.NotNull(value.Result);
                var createAccountResponse = value.Result as CreateAccountResponse;
                Assert.NotNull(createAccountResponse.IBAN);
                Assert.Equal(500, createAccountResponse.TotalAmount);
            }
        }

        [Fact]
        public async Task Create_An_Account_Existing_Customer_Success()
        {
            // ARRANGE
            var options = GetInMemoryOptions("Create_An_Account_Existing_Customer_Success");
            var customer = new Customer() { FirstName = "John", LastName = "Wick" };
            using (var context = new BankingSystemContext(options))
            {
                Seed_Create_An_Account_Existing_Customer_Success(context, customer);
                SeedIBANMasterData(context);
                var accountService = new AccountService(context);
                var controller = new AccountController(context, accountService);
                // ACT
                var jsonResult = await controller.CreateAccount(customer, 1500) as JsonResult;
                // ASSERT
                Assert.NotNull(jsonResult);
                Assert.Equal(200, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.NotNull(value.Result);
                var createAccountResponse = value.Result as CreateAccountResponse;
                Assert.NotNull(createAccountResponse.IBAN);
                Assert.Equal(1500, createAccountResponse.TotalAmount);
                var totalCustomer = context.Customers.Count();
                Assert.Equal(1, totalCustomer);
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
                Assert.Equal(400, jsonResult.StatusCode.GetValueOrDefault());
                var value = jsonResult.Value as Response;
                Assert.Equal(Entity.Constant.CUSTOMER_IS_NULL, value.Error);
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
                Assert.Equal(Entity.Constant.NO_IBAN_LEFT, value.Error);
            }
        }

    }
}
