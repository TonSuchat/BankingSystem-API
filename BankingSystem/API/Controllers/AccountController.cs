using System;
using System.Threading.Tasks;
using Entity.DBModels;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(BankingSystemContext context, IAccountService accountService) : base(context)
        {
            _accountService = accountService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAccount([FromBody] Customer customer, [FromQuery] decimal initialMoney = 0)
        {
            int statusCode = 200;
            Account result = null;
            string error = null;
            try
            {
                result = await _accountService.CreateAccount(customer, initialMoney);
            }
            catch(ArgumentException e)
            {
                Log.Error($"Failed to create an account", e);
                statusCode = 400;
                error = e.Message;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to create an account", e);
                statusCode = 500;
                error = e.Message;
            }
            return CreateResponse(statusCode, result, error);
        }

    }
}