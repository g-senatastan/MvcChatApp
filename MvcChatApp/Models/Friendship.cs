namespace MvcChatApp.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public AppUser User { get; set; }
        public AppUser Friend { get; set; }
    }
}
