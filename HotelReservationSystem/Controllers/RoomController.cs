using HotelReservationSystem.Contexts;
using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomController(HotelDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult EditRoom(int id)
        {
            var room = _context.Rooms.Include(r => r.RoomAmenities).FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new RoomViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                IsAvailable = room.IsAvailable,
                Rating = room.Rating,
                RoomAmenities = room.RoomAmenities.Select(a => new RoomAmenityViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    AmenityId = a.AmenityId,
                    HotelRoomId = a.HotelRoomId
                }).ToList()
            };

            return View("Edit",viewModel);
        }

        [HttpPost]
        public IActionResult EditRoom(RoomViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var room = _context.Rooms.Include(r => r.RoomAmenities).FirstOrDefault(r => r.Id == viewModel.Id);
                if (room == null)
                {
                    return NotFound();
                }

                room.RoomNumber = viewModel.RoomNumber;
                room.PricePerNight = viewModel.PricePerNight;
                room.IsAvailable = viewModel.IsAvailable;
                room.Rating = viewModel.Rating;

                // Update room amenities
                foreach (var amenity in viewModel.RoomAmenities)
                {
                    var existingAmenity = room.RoomAmenities.FirstOrDefault(a => a.Id == amenity.Id);
                    if (existingAmenity != null)
                    {
                        existingAmenity.Name = amenity.Name;
                        existingAmenity.Description = amenity.Description;
                    }
                    else
                    {
                        room.RoomAmenities.Add(new RoomAmenity
                        {
                            Name = amenity.Name,
                            Description = amenity.Description,
                            AmenityId = amenity.AmenityId,
                            HotelRoomId = room.Id
                        });
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Details", new { id = viewModel.Id });
            }

            return View(viewModel);
        }
    }
}
