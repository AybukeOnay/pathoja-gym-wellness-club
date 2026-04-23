using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.Mail
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest request, CancellationToken cancellationToken = default);
    }
}
