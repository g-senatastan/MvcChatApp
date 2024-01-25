namespace MvcChatApp.Models
{
    public class ChatService
    {
        public List<Message> GetChatHistory(int userId)
        {
            List<Message> chatHistory = new List<Message>();

            // Chat geçmişini döndürün.
            return chatHistory;
        }

        public void SendMessage(int userId, string message)
        {
            // Implement logic to store the message
            // You may use a database or any other storage mechanism
        }
    }
}
