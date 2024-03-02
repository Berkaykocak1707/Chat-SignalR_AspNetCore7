using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Messages
    {
        [Key]
        public int MessageID
        {
            get; set;
        }

        [Required]
        public string SenderID
        {
            get; set;
        } 

        [Required]
        public string ReceiverID
        {
            get; set;
        }

        [Required]
        public string Content
        {
            get; set;
        }

        public DateTime DateSent
        {
            get; set;
        }

        public bool IsRead
        {
            get; set;
        }

        [ForeignKey("SenderID")]
        public virtual CustomUser Sender
        {
            get; set;
        }

        [ForeignKey("ReceiverID")]
        public virtual CustomUser Receiver
        {
            get; set;
        }
    }
}
