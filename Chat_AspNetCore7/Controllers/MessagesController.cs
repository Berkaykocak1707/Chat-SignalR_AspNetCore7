using DataAccess.Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat_AspNetCore7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _messageService;
        private readonly UserManager<CustomUser> _userManager;

        public MessagesController(IMessagesRepository messageService, UserManager<CustomUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        [HttpPost("send")]
        public IActionResult Send([FromBody] Messages message)
        {
            if (message == null)
            {
                return BadRequest("Invalid message data");
            }

            _messageService.CreateMessages(message);
            return Ok();
        }

        [HttpPost("checkIsRead/{receiverId}")]
        public IActionResult CheckIsRead(string receiverId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null || receiverId == null)
            {
                return BadRequest("Invalid request data");
            }

            _messageService.CheckIsRead(userId, receiverId);
            return Ok();
        }

        [HttpGet("find/{receiverId}")]
        public ActionResult<IEnumerable<Messages>> FindAllMessages(string receiverId)
        {
            var userId = _userManager.GetUserId(User);
            var messages = _messageService.FindAllMessages(userId, receiverId);
            if (messages == null)
            {
                return NotFound();
            }
            //_messageService.CheckIsRead(userId, receiverId);
            return Ok(messages);
        }
        [HttpGet("findusers/{name}/{currentName}")]
        public ActionResult<List<CustomUser>> FindUsers(string name, string currentName)
        {
            var userList = _messageService.GetUserListWithName(name, currentName);
            if (userList == null)
            {
                return NotFound();
            }

            return Ok(userList);
        }

        [HttpGet("userConversations")]
        public ActionResult<List<CustomUser>> GetUserConversationsWithLastMessageDate(string userId)
        {
            var userConversations = _messageService.GetUserList(userId);
            if (userConversations == null)
            {
                return NotFound();
            }

            return Ok(userConversations);
        }
    }
}
