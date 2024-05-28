using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class TermsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
