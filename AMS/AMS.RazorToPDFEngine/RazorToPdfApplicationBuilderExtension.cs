using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.RazorToPDFEngine
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UsePDFEngine(this IApplicationBuilder app)
        {
            var webApp = app as WebApplication;

            if (webApp == null)
            {
                throw new Exception("Sorry, you can use Razor to pdf Engine only in a WebApplication");
            }

            PdfEngineConfiguration.Setup(webApp.Environment.WebRootPath);

            return app;

        }
    }
}
