using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcChatApp.Models;
using MvcChatApp.ViewModels;


namespace MvcChatApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ChatService _chatService;

        public ChatController(UserManager<AppUser> userManager, AppDbContext context, ChatService chatService)
        {
            _userManager = userManager;
            _context = context;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Get the list of friends
            var friends = _context.Friendships
                .Where(f => f.UserId == currentUser.Id || f.FriendId == currentUser.Id)
                .Select(f => f.UserId == currentUser.Id ? f.Friend : f.User)
                .ToList();

            return View(friends);
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Groups()
        {
            return View();
        }

        public IActionResult GetGroups()
        {
            var groupsModel = _context.Groups.Select(x => new GroupModel()
            {
                Id = x.Id,
                Name = x.Name,
                

            }).ToList();
            return View(groupsModel);
        }


    }

}
