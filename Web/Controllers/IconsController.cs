using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class IconsController : Controller
    {

        [ActionName("Boxicons")]
        public IActionResult Boxicons()
        {
            return View();
        }

        [ActionName("MaterialDesign")]
        public IActionResult MaterialDesign()
        {
            return View();
        }
              

        [ActionName("Dripicons")]
        public IActionResult Dripicons()
        {
            return View();
        }

        [ActionName("FontAwesome")]
        public IActionResult FontAwesome()
        {
            return View();
        }
    }
}
