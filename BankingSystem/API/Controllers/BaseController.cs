using Entity.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class Response
    {
        public object Result { get; set; }
        public string Error { get; set; }
    }

    public class BaseController : Controller
    {
        public readonly BankingSystemContext _context = null;

        public BaseController(BankingSystemContext context)
        {
            _context = context;
        }

        internal static IActionResult CreateResponse(int statusCode, object result = null, string error = null)
        {
            var response = new Response() { Result = result, Error = error };
            return new JsonResult(response) { StatusCode = statusCode };
        }
    }
}