using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models
{
    public class NotificationApplicationUser
    {
        public string NotificationId { get; set; }
        public Notification Notification { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
