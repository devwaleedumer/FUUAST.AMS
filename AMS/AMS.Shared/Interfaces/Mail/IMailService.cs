using AMS.MODELS.Models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Interfaces.Mail
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request, CancellationToken ct);
    }
}
