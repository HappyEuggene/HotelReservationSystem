using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
