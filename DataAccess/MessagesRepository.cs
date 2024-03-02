using DataAccess.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MessagesRepository : RepositoryBase<Messages>, IMessagesRepository
    {
        public MessagesRepository(RepositoryContext context) : base(context)
        {
        }

        public void CheckIsRead(string SenderID, string ReceiverID)
        {
            var messagesToUpdate = _context.Messages
                               .Where(m => m.SenderID == ReceiverID && m.ReceiverID == SenderID && !m.IsRead)
                               .ToList();

            if (!messagesToUpdate.Any())
            {
                return;
            }

            foreach (var message in messagesToUpdate)
            {
                message.IsRead = true;
            }

            _context.SaveChanges();
        }

        public int CreateMessages(Messages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _context.Messages.Add(message);
            _context.SaveChanges();

            return message.MessageID;
        }

        public IQueryable<Messages> FindAllMessages(string SenderID, string ReceiverID)
        {
            return _context.Messages
                   .Where(m => (m.SenderID == SenderID && m.ReceiverID == ReceiverID)
                            || (m.SenderID == ReceiverID && m.ReceiverID == SenderID))
                   .OrderBy(m => m.DateSent);
        }

        public async Task<List<Messages>> GetUnreadMessages(string receiverId, string senderId)
        {
            return await _context.Messages
                                 .Where(m => m.ReceiverID == receiverId && m.SenderID == senderId && !m.IsRead)
                                 .ToListAsync();
        }

        public List<CustomUser> GetUserList(string currentUserId)
        {
            var currentUserMessages = _context.Messages
            .Where(message => message.SenderID == currentUserId || message.ReceiverID == currentUserId)
            .Select(message => message.SenderID == currentUserId ? message.ReceiverID : message.SenderID)
            .Distinct()
            .ToList();

            var allUsers = _context.Users
                .Where(user => user.Id != currentUserId)
                .OrderByDescending(user => currentUserMessages.Contains(user.Id)) // Mevcut kullanıcı ile mesajlaşan kullanıcılar en üstte
                .ToList();
            return allUsers;
        }

        public List<CustomUser> GetUserListWithName(string name, string currentName)
        {
            if (name == null || name == "All")
            {
                return _context.Users
                               .Where(u => u.UserName != currentName)
                               .ToList();
            }
            else
            {
                var users = _context.Users
                                    .Where(u => (u.FirstName.Contains(name) || u.LastName.Contains(name) || u.UserName.Contains(name))
                                                && u.UserName != currentName)
                                    .ToList();

                return users;
            }
        }

        public void UpdateMessages(Messages message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            // Burada nesnenin durumunu kontrol etmek için ek adımlar gerekebilir.
            _context.Messages.Update(message);
            _context.SaveChanges();
        }

        public void UserStatusChange(string userId, bool status)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null) // Kullanıcı varsa
            {
                user.IsOnline = status; // Kullanıcının durumunu güncelle
                _context.SaveChanges(); // Değişiklikleri kaydet
            }
        }
    }
}
