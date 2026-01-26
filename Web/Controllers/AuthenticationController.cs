using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class AuthenticationController : Controller
    {

        [ActionName("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [ActionName("lockscreen")]
        public IActionResult lockscreen()
        {
            return View();
        }

        [ActionName("Recoverpassword")]
        public IActionResult Recoverpassword()
        {
            return View();
        }

        [ActionName("Registretion")]
        public IActionResult Registretion()
        {
            return View();
        }

        [ActionName("ConfirmMail")]
        public IActionResult ConfirmMail()
        {
            return View();
        }
        [ActionName("Emailverification")]
        public IActionResult Emailverification()
        {
            return View();
        }
        [ActionName("Twostepvarification")]
        public IActionResult Twostepvarification()
        {
            return View();
        }

        

    }
}
