using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcChatApp.Models;

namespace MvcChatApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<AppUser> userManager, AppDbContext context)
        {

            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddFriend()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string searchString)
        {
            var user = await _userManager.FindByNameAsync(searchString);

            if (user == null)
            {
                // Handle the case when the user is not found
                // You can redirect or display a message accordingly
                return View();
            }

            // Check if the users are already friends
            var currentUser = await _userManager.GetUserAsync(User);
            var isFriend = _context.Friendships
                .Any(f => (f.UserId == currentUser.Id && f.FriendId == user.Id) ||
                          (f.UserId == user.Id && f.FriendId == currentUser.Id));

            if (!isFriend)
            {
                // Friendship does not exist, create a new one
                var friendship = new Friendship
                {
                    UserId = currentUser.Id,
                    FriendId = user.Id
                };

                _context.Friendships.Add(friendship);
                await _context.SaveChangesAsync();

                // Display a success message
                TempData["FriendAdded"] = $"{user.UserName} artık senin arkadaşın!";
            }
            else
            {
                // Display a message indicating they are already friends
                TempData["AlreadyFriends"] = $"{user.UserName} zaten arkadaşın!";
            }

            return View(user);
        }

        public async Task< IActionResult> Friend()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Get the list of friends
            var friends = _context.Friendships
                .Where(f => f.UserId == currentUser.Id || f.FriendId == currentUser.Id)
                .Select(f => f.UserId == currentUser.Id ? f.Friend : f.User)
                .ToList();

            return View(friends);
        }
    }
}
