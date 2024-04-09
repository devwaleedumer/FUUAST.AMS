using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Interfaces.Mail
{
    public interface IEmailTemplateService 
    {
        string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
    }
}
