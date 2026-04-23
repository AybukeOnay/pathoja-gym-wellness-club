using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.Mail
{
    public class MailSettings
    {
        public string From { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool EnableSsl { get; set; }
        public string AdminTo { get; set; } = null!;
    }
}
