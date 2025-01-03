using AMS.SERVICES.Reporting.IService;
using AMS.WordReportingEngine.Documents;


namespace AMS.SERVICES.Reporting.Services
{
    public class WordToPdfGenerator : IWordReportGenerator
    {
       
        public async Task<byte[]> ConvertWordToPdfAsync(string input)
        {
            WriterDocument writer = new WriterDocument(input);
            var filePath = await writer.ExportAsPDF();
            try
            {
                var bytes =  await File.ReadAllBytesAsync(filePath);
                return bytes;
            }
            finally
            {
                if (File.Exists(filePath))
                     File.Delete(filePath);
            }
        }
    }
}
