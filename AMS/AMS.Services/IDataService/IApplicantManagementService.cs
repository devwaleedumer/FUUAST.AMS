using AMS.MODELS;
using AMS.MODELS.ApplicantManagement;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.Filters;
using AMS.MODELS.MeritList;


namespace AMS.SERVICES.IDataService
{
    public interface IApplicantManagementService
    {
        Task<List<ApplicantInfoList>> GetAllApplicantDetails(ApplicantInfoRequest user);
        Task<List<Applicantmanagementresponse>> UpdateApplicantDetails(updateApplicantRequest request);
        Task<MeritListDataModel> GetMeritListData(int meritListId);
        Task<PaginationResponse<MeritListResponse>> GetAllMeritListDetailsByFilter(LazyLoadEvent request, CancellationToken ct);
        Task GenerateMeritList(GenerateMeritListRequest request);
    }
}
