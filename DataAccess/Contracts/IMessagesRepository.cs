using Entities.Models;

namespace DataAccess.Contracts
{
    public interface IMessagesRepository : IRepositoryBase<Messages>
    {
        IQueryable<Messages> FindAllMessages(string SenderID,string ReceiverID);
        int CreateMessages(Messages message);
        void UpdateMessages(Messages message);
        void CheckIsRead(string SenderID, string ReceiverID);
        List<CustomUser> GetUserList(string currentUserId);
        List<CustomUser> GetUserListWithName(string name, string currentName);
        Task<List<Messages>> GetUnreadMessages(string receiverId, string senderId);
        void UserStatusChange(string userId, bool status);
    }
}
