using Entities.Models;

namespace Chat_AspNetCore7.Models
{
    public class ChatViewModel
    {
        public List<CustomUser> customUsers
        {
        get; set; } 
        public CustomUser currentUser
        {
        get; set; }
    }
}
