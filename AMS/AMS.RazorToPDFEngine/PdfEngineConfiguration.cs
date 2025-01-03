using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.RazorToPDFEngine
{
    public static class PdfEngineConfiguration
    {
        private static string _PdfEnginePath;
        internal static string pdfEnginePath
        {
            get
            {
                if (string.IsNullOrEmpty(_PdfEnginePath))
                {
#if NET45
                    _PdfEngineUrl = System.Configuration.ConfigurationManager.AppSettings["pdfEngineUrl"];
#endif
                }
                return _PdfEnginePath;
            }
        }
        /// <summary>
        /// Setup PdfEngine library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfEnginPath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "PdfEngine". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(IHostingEnvironment env, string wkhtmltopdfEnginPath = "PdfEngine")
        {
            var pdfEnginePath = Path.Combine(env.WebRootPath, wkhtmltopdfEnginPath);

            if (!Directory.Exists(pdfEnginePath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + pdfEnginePath);
            }

            _PdfEnginePath = pdfEnginePath;
        }


        /// <summary>
        /// Setup pdfEngine library
        /// </summary>
        /// <param name="rootPath">The path to the web-servable application files.</param>
        /// <param name="wkhtmltopdfEnginPath">Optional. Relative path to the directory containing wkhtmltopdf.exe. Default is "pdfEngine". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(string rootPath, string wkhtmltopdfEnginPath = "pdfEngine")
        {
            var pdfEnginePath = Path.Combine(rootPath, wkhtmltopdfEnginPath);

            if (!Directory.Exists(pdfEnginePath))
            {
                throw new ApplicationException("Folder containing wkhtmltopdf.exe not found, searched for " + pdfEnginePath);
            }

            _PdfEnginePath = pdfEnginePath;
        }

    }
}
