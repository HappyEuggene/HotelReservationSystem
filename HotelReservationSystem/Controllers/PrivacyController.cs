using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
