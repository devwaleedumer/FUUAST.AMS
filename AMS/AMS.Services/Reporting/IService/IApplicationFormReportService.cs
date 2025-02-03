namespace AMS.SERVICES.Reporting.IService
{
    public interface IApplicationFormReportService
    {
        Task<byte[]> GenerateUGApplicationFormPdfReportAsync(int applicationFormId);
    }
}