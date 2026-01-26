using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AppsController : Controller
    {
        [ActionName("Calendar")]
        public IActionResult Calendar()
        {
            return View();
        }

        [ActionName("Chat")]
        public IActionResult Chat()
        {
            return View();
        }      

        [ActionName("EmailRead")]
        public IActionResult EmailRead()
        {
            return View();
        }

        [ActionName("Email-inbox")]
        public IActionResult Emailinbox()
        {
            return View();
        }

        

    }
}
