using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class PagesController : Controller
    {

        [ActionName("Starter")]
        public IActionResult Starter()
        {
            return View();
        }

        [ActionName("ProfileSimple")]
        public IActionResult ProfileSimple()
        {
            return View();
        }
             

        [ActionName("Timeline")]
        public IActionResult Timeline()
        {
            return View();
        }

        [ActionName("preloader")]
        public IActionResult preloader()
        {
            return View();
        }

        [ActionName("Pricing")]
        public IActionResult Pricing()
        {
            return View();
        }

        [ActionName("Maintenance")]
        public IActionResult Maintenance()
        {
            return View();
        }

        [ActionName("ComingSoon")]
        public IActionResult ComingSoon()
        {
            return View();
        }

        [ActionName("Errors500")]
        public IActionResult Errors500()
        {
            return View();
        }

        [ActionName("Errors404")]
        public IActionResult Errors404()
        {
            return View();
        }

        [ActionName("Gallery")]
        public IActionResult Gallery()
        {
            return View();
        }

        [ActionName("Team")]
        public IActionResult Team()
        {
            return View();
        }

        [ActionName("Invoice")]
        public IActionResult Invoice()
        {
            return View();
        }

    }
}
