using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        internal static IActionResult CreateResponse(int statusCode, object result = null, string error = null)
        {
            var response = new Response() { Result = result, Error = error };
            return new JsonResult(response) { StatusCode = statusCode };
        }
    }
}