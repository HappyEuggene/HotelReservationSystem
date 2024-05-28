using HotelReservationSystem.Contexts;
using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers;
public class ReservationController : Controller
{
    private readonly HotelDbContext _context;
    private readonly UserManager<User> _userManager;

    public ReservationController(HotelDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create(int hotelId, int roomId)
    {
        var model = new ReservationViewModel
        {
            HotelId = hotelId,
            RoomId = roomId
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);

            var room = await _context.Rooms.FindAsync(model.RoomId);
            if (room == null || !room.IsAvailable)
            {
                ModelState.AddModelError(string.Empty, "Room is not available.");
                return View(model);
            }

            var days = (model.EndDate - model.StartDate).Days;
            var totalPrice = days * room.PricePerNight;

            var reservation = new Reservation
            {
                UserId = user.Id,
                RoomId = model.RoomId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TotalPrice = totalPrice,
                Status = "Pending"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var hotelReservation = new HotelReservation
            {
                HotelId = model.HotelId,
                ReservationId = reservation.Id
            };

            _context.HotelReservations.Add(hotelReservation);
            room.IsAvailable = false;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }
    [Authorize]
    public async Task<IActionResult> MyReservations()
    {
        var user = await _userManager.GetUserAsync(User);
        var reservations = await _context.Reservations
            .Include(r => r.HotelReservations)
                .ThenInclude(hr => hr.Hotel)
            .Include(r => r.Room)
            .Where(r => r.UserId == user.Id)
            .ToListAsync();

        return View(reservations);
    }
}
