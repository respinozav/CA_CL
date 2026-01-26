using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AdvancKitController : Controller
    {
        [ActionName("Carousel")]
        public IActionResult Carousel()
        {
            return View();
        }

        [ActionName("Notifications")]
        public IActionResult Notifications()
        {
            return View();
        }

        [ActionName("SweetAlerts")]
        public IActionResult SweetAlerts()
        {
            return View();
        }

        [ActionName("SwiperSlider")]
        public IActionResult SwiperSlider()
        {
            return View();
        }
    }
}
