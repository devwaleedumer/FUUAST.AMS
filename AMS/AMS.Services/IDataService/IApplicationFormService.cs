using AMS.MODELS.ApplicationForm;
using AMS.MODELS.Dashboard;

namespace AMS.SERVICES.IDataService
{
    public interface IApplicationFormService
    {
        Task<CreateApplicationFormResponse> CreateApplicationForm(CreateApplicationFormRequest request, CancellationToken ct);
        Task<string> SubmitApplicationForm(SubmitApplicationFormRequest request, CancellationToken ct);
        Task<SubmitApplicationResponse> GetSubmittedApplication(CancellationToken ct);
        Task<string> EditSubmittedApplication(EditSubmitApplicationFormRequest request, CancellationToken ct);
        Task<ApplicantDashboardResponse> GetApplicationFormStatus(int userId, CancellationToken ct);

    }
}