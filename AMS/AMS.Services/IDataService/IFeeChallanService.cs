using AMS.MODELS.FeeChallan;

namespace AMS.SERVICES.IDataService
{
    public interface IFeeChallanService
    {
        Task<FeeChallanReportDto> GetFeeChallanData(int applicantId, CancellationToken ct);
        Task<bool> FeeChallanExists(int applicantId, CancellationToken ct);
        Task UploadFeeChallanImage(int feeChallanId,FeeChallanSubmissionRequest request, CancellationToken ct);
    }
}