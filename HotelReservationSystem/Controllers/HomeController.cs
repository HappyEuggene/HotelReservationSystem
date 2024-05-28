using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;
using HotelReservationSystem.Contexts;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace HotelReservationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public HomeController(HotelDbContext context, UserManager<User> userManager, BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string city, DateTime? arrivalDate, int? roomCount)
        {
            var hotels = _context.Hotels.Include(h => h.Address)
                                        .Include(h => h.HotelReservations)
                                            .ThenInclude(hr => hr.Reservation)
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(city))
            {
                hotels = hotels.Where(h => h.Address.City.Contains(city));
            }

            if (roomCount.HasValue)
            {
                hotels = hotels.Where(h => h.RoomCount >= roomCount.Value);
            }

            if (arrivalDate.HasValue)
            {
                hotels = hotels.Where(h => !h.HotelReservations.Any(hr =>
                    hr.Reservation.StartDate <= arrivalDate.Value && hr.Reservation.EndDate >= arrivalDate.Value));
            }

            var results = await hotels.ToListAsync();
            return View("Hotels", results);
        }

        public async Task<IActionResult> Hotels()
        {
            var hotels = await _context.Hotels.Include(h => h.Rooms)
                                              .Include(h => h.Reviews)
                                              .ToListAsync();
            return View(hotels);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.Rooms)  
                .Include(h => h.Reviews)
                .Include(h => h.Address)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }


            hotel.Rating = hotel.AverageRating;

            return View(hotel);
        }

        public IActionResult Create()
        {
            var hotelViewModel = new HotelViewModel
            {
                Hotel = new Hotel(),
                Address = new Address(),
                Rooms = new List<RoomViewModel> { new RoomViewModel() },
                BulkAddRooms = new BulkAddRoomsViewModel()
            };
            return View(hotelViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Picture != null)
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient("forhotel");
                   
                    var blobClient = containerClient.GetBlobClient(model.Picture.FileName);

                    await using (var stream = model.Picture.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "image/jpeg" });
                    }

                    model.Hotel.PictureUrl = blobClient.Uri.ToString();
                }

                var hotel = model.Hotel;

                var address = model.Address;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                hotel.AddressId = address.Id;

                _context.Hotels.Add(hotel);
                await _context.SaveChangesAsync();

                if (model.Hotel.RoomCount > 0)
                {
                    var rooms = new List<Room>();
                    for (int i = 0; i < model.Hotel.RoomCount; i++)
                    {
                        rooms.Add(new Room
                        {
                            HotelId = hotel.Id,
                            RoomNumber = i + 1,
                            IsAvailable = true,
                            PricePerNight = model.BulkAddRooms.PricePerNight,
                        });
                    }
                    await _context.Rooms.AddRangeAsync(rooms);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Authorize]
        public IActionResult AddReview(int hotelId)
        {
            var reviewViewModel = new ReviewViewModel { HotelId = hotelId };
            return View(reviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var review = new Review
                {
                    UserId = user.Id,
                    HotelId = model.HotelId,
                    Rating = model.Rating,
                    Comment = model.Comment,
                    Date = DateTime.Now
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                var hotel = await _context.Hotels.Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == model.HotelId);
                if (hotel != null)
                {
                    hotel.Rating = hotel.AverageRating;
                    _context.Hotels.Update(hotel);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Details", new { id = model.HotelId });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _context.Hotels.Include(h => h.Address).FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            var viewModel = new HotelViewModel
            {
                Hotel = hotel,
                Address = hotel.Address != null ? new Address
                {
                    Id = hotel.Address.Id,
                    Country = hotel.Address.Country,
                    City = hotel.Address.City,
                    Region = hotel.Address.Region,
                    Street = hotel.Address.Street,
                    PostalCode = hotel.Address.PostalCode
                } 
                : new Address(),
                Rooms = hotel.Rooms.Select(r => new RoomViewModel
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    IsAvailable = r.IsAvailable,
                    PricePerNight = (decimal)r.PricePerNight
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelViewModel model)
        {
            if (id != model.Hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Picture != null)
                    {
                        var containerClient = _blobServiceClient.GetBlobContainerClient("hotelcontainer");
                        await containerClient.CreateIfNotExistsAsync();
                        var blobClient = containerClient.GetBlobClient(model.Picture.FileName);

                        await using (var stream = model.Picture.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        model.Hotel.PictureUrl = blobClient.Uri.ToString();
                    }

                    var hotel = await _context.Hotels.Include(h => h.Address).FirstOrDefaultAsync(h => h.Id == id);

                    hotel.Name = model.Hotel.Name;
                    hotel.RoomCount = model.Hotel.RoomCount;
                    hotel.VacationHotel = model.Hotel.VacationHotel;
                    hotel.Description = model.Hotel.Description;
                    hotel.Contact = model.Hotel.Contact;
                    hotel.PictureUrl = model.Hotel.PictureUrl;

                    if (hotel.Address == null)
                    {
                        hotel.Address = new Address();
                    }

                    hotel.Address.Country = model.Address.Country;
                    hotel.Address.City = model.Address.City;
                    hotel.Address.Region = model.Address.Region;
                    hotel.Address.Street = model.Address.Street;
                    hotel.Address.PostalCode = model.Address.PostalCode;

                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(model.Hotel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Rooms.RemoveRange(hotel.Rooms);

            _context.Hotels.Remove(hotel);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}
