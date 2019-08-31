using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ActionController : BaseController
    {

        public ActionController(BankingSystemContext context) : base(context)
        {

        }

        [HttpPut()]
        public IActionResult Index()
        {
            return View();
        }
    }
}