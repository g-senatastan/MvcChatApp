using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MvcChatApp.Models;
using System.Text.RegularExpressions;

namespace MvcChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{Context.User.Identity.Name} has left the group");
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateGroup(string groupName)
        {
            // Grup adını kontrol etmek için boş olup olmadığını kontrol edebilirsiniz
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);

                if (existingGroup == null)
                {
                    var newGroup = new Models.Group() { Name = groupName };
                    _context.Groups.Add(newGroup);
                    await _context.SaveChangesAsync();

                    await Clients.All.SendAsync("GroupCreated", groupName);
                }
                else
                {
                    await Clients.Caller.SendAsync("GroupExists", groupName);
                }
            }
        }
    }
}
