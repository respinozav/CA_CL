using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class FormsController : Controller
    {

        [ActionName("BasicElements")]
        public IActionResult BasicElements()
        {
            return View();
        }

        [ActionName("Advanced")]
        public IActionResult Advanced()
        {
            return View();
        }

      
       
    }
}
