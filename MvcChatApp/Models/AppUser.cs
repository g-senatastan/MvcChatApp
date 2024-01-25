using Microsoft.AspNetCore.Identity;

namespace MvcChatApp.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

        public List<Friendship> Friendships { get; set; }
        
        
        
    }
}
