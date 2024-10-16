using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.ApplicationForm.ApplicantDegree;
using AMS.MODELS.Program;

namespace AMS.SERVICES.IDataService
{
    public interface IApplicantService
    {
        //Personal Information
        Task<CreateApplicantPSInfoResponse> AddApplicantPersonalInformation(CreateApplicantPSInfoRequest request, CancellationToken cancellationToken);
        Task<ApplicantPSInfoResponse> GetApplicantPersonalInformation(CancellationToken cancellationToken);
        Task<UpdateApplicantPSInfoResponse> UpdateApplicantPersonalInformation(UpdateApplicantPSInfoRequest request, CancellationToken cancellationToken);
        //Degrees
        Task<List<CreateApplicantDegreeResponse>> AddApplicantDegrees(CreateApplicantDegreeListRequest request, CancellationToken ct);
        Task<List<ApplicantDegreeResponse>> GetApplicantDegrees(CancellationToken ct);
        Task<List<EditApplicantDegreeResponse>> EditApplicantDegrees(EditApplicantDegreeListRequest request, CancellationToken ct);



    }
}