using AMS.MODELS.ApplicationForms.Ug;
using Microsoft.AspNetCore.Http;
namespace AMS.SERVICES.IDataService
{
    public interface IApplicationFormService
    {
        Task<string> AddApplicationForm(ApplicationRequest request);
    }
}
