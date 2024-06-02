using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using HotelReservationSystem.Contexts;
using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;

        public RoomController(HotelDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                                     .Include(r => r.RoomAmenities)
                                     .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            var amenities = await _context.RoomAmenities.ToListAsync();
            ViewBag.Amenities = new MultiSelectList(amenities, "Id", "Name", room.RoomAmenities.Select(a => a.Id));

            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room, int[] selectedAmenities, IFormFile Image)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRoom = await _context.Rooms
                                                     .Include(r => r.RoomAmenities)
                                                     .FirstOrDefaultAsync(r => r.Id == id);

                    if (existingRoom == null)
                    {
                        return NotFound();
                    }

                    existingRoom.PricePerNight = room.PricePerNight;
                    existingRoom.IsAvailable = room.IsAvailable;
                    existingRoom.Rating = room.Rating;
                    existingRoom.RoomNumber = room.RoomNumber;
                    existingRoom.HotelId = room.HotelId;

                    // Add new room amenities without clearing the existing ones
                    var currentAmenityIds = existingRoom.RoomAmenities.Select(a => a.Id).ToList();
                    foreach (var amenityId in selectedAmenities)
                    {
                        if (!currentAmenityIds.Contains(amenityId))
                        {
                            var amenity = await _context.RoomAmenities.FindAsync(amenityId);
                            if (amenity != null)
                            {
                                existingRoom.RoomAmenities.Add(amenity);
                            }
                        }
                    }

                    if (Image != null)
                    {
                        var containerClient = _blobServiceClient.GetBlobContainerClient("forhotelrooms");

                        var blobClient = containerClient.GetBlobClient(Image.FileName);

                        await using (var stream = Image.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "image/jpeg" });
                        }

                        existingRoom.PictureUrl = blobClient.Uri.ToString();
                    }

                    _context.Update(existingRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Hotels", "Home");
            }

            var amenities = await _context.RoomAmenities.ToListAsync();
            ViewBag.Amenities = new MultiSelectList(amenities, "Id", "Name", selectedAmenities);
            return View(room);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
