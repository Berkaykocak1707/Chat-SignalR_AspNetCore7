using Business.Contracts;
using Chat_AspNetCore7.Models;
using DataAccess.Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat_AspNetCore7.Controllers
{
	public class ChatController : Controller
	{
		private readonly IServiceManager _services;
        private readonly IMessagesRepository _messageService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public ChatController(IServiceManager services, IMessagesRepository messageService, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            _services = services;
            _messageService = messageService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
		public async Task<ActionResult> IndexAsync()
		{
            var user = await _userManager.GetUserAsync(User);
            var userList = _messageService.GetUserList(user.Id);
			return View(new ChatViewModel() 
            { 
                currentUser=user,
                customUsers = userList
            
            });
		}
	}
}
