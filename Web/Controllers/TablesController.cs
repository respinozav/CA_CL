using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class TablesController : Controller
    {
        [ActionName("BoostrepTables")]
        public IActionResult BoostrepTables()
        {
            return View();
        }      
              
        [ActionName("Datatables")]
        public IActionResult Datatables()
        {
            return View();
        }
    }
}
