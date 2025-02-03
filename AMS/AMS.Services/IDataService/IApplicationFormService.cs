using AMS.MODELS.ApplicationForm;
using AMS.MODELS.Dashboard;

namespace AMS.SERVICES.IDataService
{
    public interface IApplicationFormService
    {
        Task<CreateApplicationFormResponse> CreateApplicationForm(CreateApplicationFormRequest request, CancellationToken ct);
        Task<string> AddApplicationFormPrograms(SubmitApplicationFormRequest request, CancellationToken ct);
        Task<SubmitApplicationResponse> GetApplicationFormPrograms(CancellationToken ct);
        Task<string> EditApplicationFormPrograms(EditSubmitApplicationFormRequest request, CancellationToken ct);
        Task<ApplicantDashboardResponse> GetApplicationFormStatus(int userId, CancellationToken ct);
        Task<ApplicationDetailResponse> ApplicationDetailsByApplicantId(int applicantId, CancellationToken ct);

    }
}