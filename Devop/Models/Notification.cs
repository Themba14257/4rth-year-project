using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<NotificationApplicationUser> NotificationApplicationUsers { get; set; }
    }
}
