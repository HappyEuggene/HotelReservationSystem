using HotelReservationSystem.Contexts;
using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelReservationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(HotelDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult RoomAmenities()
        {
            var amenities = _context.RoomAmenities.Include(r => r.HotelRooms).ThenInclude(h => h.Hotel).ToList();
            return View(amenities);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoomAmenity()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRoomAmenity(RoomAmenity model)
        {
            if (ModelState.IsValid)
            {
                _context.RoomAmenities.Add(model);
                _context.SaveChanges();

                return RedirectToAction(nameof(RoomAmenities));
            }

            var amenities = _context.RoomAmenities.ToList();
            return View(amenities);
        }

        [HttpPost]
        public IActionResult DeleteRoomAmenity(int id)
        {
            var roomAmenity = _context.RoomAmenities.Find(id);
            if (roomAmenity != null)
            {
                _context.RoomAmenities.Remove(roomAmenity);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(RoomAmenities));
        }

        public async Task<IActionResult> AllUsers()
        {
            var usersWithRoles = await (from user in _context.Users
                                        select new UserWithRolesViewModel
                                        {
                                            UserId = user.Id,
                                            UserName = user.UserName,
                                            Roles = (from userRole in _context.UserRoles
                                                     join role in _context.Roles on userRole.RoleId equals role.Id
                                                     where userRole.UserId == user.Id
                                                     select role.Name).ToList()
                                        }).ToListAsync();

            return View(usersWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleToHotelManager(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeRolesResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return RedirectToAction(nameof(Index));
            }

            if (!await _roleManager.RoleExistsAsync("HotelManager"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("HotelManager"));
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to create role");
                    return RedirectToAction(nameof(Index));
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "HotelManager");
            if (!addRoleResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user to role");
            }

            return RedirectToAction(nameof(AllUsers));
        }
    }
}
