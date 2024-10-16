using AMS.MODELS.ApplicationForm;

namespace AMS.SERVICES.IDataService
{
    public interface IApplicationFormService
    {
        Task<CreateApplicationFormResponse> CreateApplicationForm(CreateApplicationFormRequest request, CancellationToken ct);
        Task<string> SubmitApplicationForm(SubmitApplicationFormRequest request, CancellationToken ct);

    }
}