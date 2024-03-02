using DataAccess.Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Chat_AspNetCore7.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, bool> OnlineUsers = new ConcurrentDictionary<string, bool>();
        private static readonly Dictionary<string, HashSet<string>> GroupUsers = new Dictionary<string, HashSet<string>>();
        private readonly IMessagesRepository _messageService;
        private readonly UserManager<CustomUser> _userManager;

        public ChatHub(IMessagesRepository messageService, UserManager<CustomUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User);
            OnlineUsers.TryAdd(user.Id, true);
            _messageService.UserStatusChange(user.Id, true);
            await Clients.All.SendAsync("UserStatusChanged", user.Id, true);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            bool removed;
            OnlineUsers.TryRemove(user.Id, out removed);
            var groups = GroupUsers.Where(g => g.Value.Contains(Context.ConnectionId)).Select(g => g.Key).ToList();
            foreach (var groupName in groups)
            {
                if (GroupUsers.ContainsKey(groupName))
                {
                    GroupUsers[groupName].Remove(Context.ConnectionId);

                    if (GroupUsers[groupName].Count == 0)
                    {
                        GroupUsers.Remove(groupName);
                    }
                }

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
            }
            _messageService.UserStatusChange(user.Id, false);
            await Clients.All.SendAsync("UserStatusChanged", user.Id, false);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task MarkMessagesAsRead(string receiverId, string senderId)
            {
            var groups = GroupUsers.Where(g => g.Value.Contains(senderId)).Select(g => g.Key).ToList();

            foreach (var groupName in groups)
            {
                if (GroupUsers.ContainsKey(groupName))
                {
                    if (GroupUsers[groupName].Contains(receiverId))
                    {
                        continue;
                    }
                    GroupUsers[groupName].Remove(senderId);

                    if (GroupUsers[groupName].Count == 0)
                    {
                        GroupUsers.Remove(groupName);
                    }
                    await Groups.RemoveFromGroupAsync(senderId, groupName);
                    await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
                }
            }
            var group = DoesGroupExist(receiverId,senderId);
            if (group == false)
            {
                await AddToGroup(senderId, receiverId,false);
            }
            else
            {
                await AddToGroup(senderId,receiverId,true);
            }
            var messages = await _messageService.GetUnreadMessages(senderId, receiverId);
            foreach (var message in messages)
            {
                message.IsRead = true;
                _messageService.UpdateMessages(message);
                await Clients.User(receiverId).SendAsync("UpdateMessageStatus", message.MessageID, true);
            }
        }

        public async Task SendMessage(string userName, string receiverId, string message)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userID = await _userManager.GetUserIdAsync(user);
            bool isActiveChat = OnlineUsers.ContainsKey(receiverId);
            if (isActiveChat == true)
            {
                var group1 = DoesGroupExist(receiverId, userID);
                var group2 = DoesGroupExist(userID, receiverId);
                if (group1 == false && group2 == false)
                {
                    isActiveChat = false;
                }
                else
                {
                    bool isHereReceiver = IsUserInGroup(userID, receiverId, receiverId);
                    bool isHereUser = IsUserInGroup(receiverId, userID, userID);
                    if (isHereReceiver == true && isHereUser == true)
                    {
                        isActiveChat = true;
                    }
                    else
                    {
                        isActiveChat= false;
                    }
                }
            }
            var messageSave = new Messages
            {
                ReceiverID = receiverId,
                SenderID = userID,
                Content = message,
                IsRead = isActiveChat,
                DateSent = DateTime.Now
            };
            var messageId = _messageService.CreateMessages(messageSave);
            await Clients.Users(receiverId, userID).SendAsync("ReceiveMessage", userID, message, userName, messageId, isActiveChat);
        }
        public async Task AddToGroup(string senderId, string receiverId, bool isActive)
        {
            if (isActive == false)
            {
                string groupName = senderId + receiverId;
                if (!GroupUsers.ContainsKey(groupName))
                {
                    GroupUsers[groupName] = new HashSet<string>();
                }

                GroupUsers[groupName].Add(senderId);

                await Groups.AddToGroupAsync(senderId, groupName);
                await Clients.Group(groupName).SendAsync("Send", $"{senderId} has joined the group {groupName}.");
            }
            else
            {
                string groupName = receiverId + senderId;
                GroupUsers[groupName].Add(senderId);

                await Groups.AddToGroupAsync(senderId, groupName);
                await Clients.Group(groupName).SendAsync("Send", $"{senderId} has joined the group {groupName}.");
            }

        }

        public bool IsUserInGroup(string senderId, string receiverId,string who)
        {
            string groupName = senderId + receiverId;
            var sorgu1 = GroupUsers.ContainsKey(groupName) && GroupUsers[groupName].Contains(who);
            string groupName2 = receiverId + senderId;
            var sorgu2 = GroupUsers.ContainsKey(groupName2) && GroupUsers[groupName2].Contains(who);
            if (sorgu1 == true || sorgu2 == true)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
        public bool DoesGroupExist(string senderId, string receiverId)
        {
            string groupName = senderId + receiverId;
            return GroupUsers.ContainsKey(groupName) && GroupUsers[groupName].Count > 0;
        }
    }

}
