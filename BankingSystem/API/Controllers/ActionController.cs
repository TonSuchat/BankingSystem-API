using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.DBModels;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using static Entity.Models.ActionModels;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ActionController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IActionService _actionService;

        public ActionController(BankingSystemContext context, IAccountService accountService, IActionService actionService) : base(context)
        {
            _accountService = accountService;
            _actionService = actionService;
        }

        [HttpPost(), ActionName("Deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            int statusCode = 200;
            DepositResponse result = null;
            string error = null;
            try
            {
                result = await _actionService.Deposit(request.IBAN, request.DepositMoney, request.Remark);
            }
            catch (ArgumentException e)
            {
                Log.Error($"Failed to deposit", e);
                statusCode = 400;
                error = e.Message;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to deposit", e);
                statusCode = 500;
                error = e.Message;
            }
            return CreateResponse(statusCode, result, error);
        }
    }
}