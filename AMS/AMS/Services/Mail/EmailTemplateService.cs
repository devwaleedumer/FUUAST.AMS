using AMS.Interfaces.Mail;
using RazorEngineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.EmailTemplateService
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IWebHostEnvironment _env;
        public EmailTemplateService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            // get template directory
            string baseDirectory = _env.ContentRootPath;
            string tmplFolder = Path.Combine(baseDirectory, "Email Templates");
            string template = GetTemplate(templateName,tmplFolder);

            // using RazorEngine Core fore template rendering
            IRazorEngine razorEngine = new RazorEngine();
            
            // 
            IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

            return modifiedTemplate.Run(mailTemplateModel);
            
        }

        public static string GetTemplate(string templateName, string folder)
        {
            
            // for deployement time uncomment it
            //string baseDirectory = AppContext.BaseDirectory;

            string filePath = Path.Combine(folder, $"{templateName}.cshtml");

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            string mailText = sr.ReadToEnd();
            sr.Close();

            return mailText;
        }
    }
}
