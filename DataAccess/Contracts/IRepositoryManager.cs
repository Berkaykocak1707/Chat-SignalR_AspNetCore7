using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IRepositoryManager
    {
        IMessagesRepository MessagesRepository
        {
        get; 
        }

        void Save();
    }
}
