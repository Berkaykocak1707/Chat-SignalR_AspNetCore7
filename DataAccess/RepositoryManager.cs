using DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IMessagesRepository _messagesRepository;

        public RepositoryManager(RepositoryContext context, IMessagesRepository messagesRepository)
        {
            _context = context;
            _messagesRepository = messagesRepository;
        }

        public IMessagesRepository MessagesRepository => _messagesRepository;

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
