using AMS.DATA;
using AMS.MODELS;
using AMS.MODELS.ApplicantManagement;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.Filters;
using AMS.MODELS.MeritList;
using AMS.SERVICES.Dapper;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Enums.AMS;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using Dapper;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AMS.SERVICES.DataService
{
    public class ApplicantManagementService(AMSContext context, IDapperService dapperService) : IApplicantManagementService
    {
        private readonly AMSContext _context = context;
        private readonly IDapperService _dapperService = dapperService;


        public async Task <List<ApplicantInfoList>> GetAllApplicantDetails(ApplicantInfoRequest user)
        {
            DynamicParameters parameters = new();
           
            parameters.Add("@UserName",user.UserName);
           
            parameters.Add("@EMAIL", user.Email);
            parameters.Add("@VerificationStatusEid", user.VerificationStatusEid);
            ////    parameters.Add("@PageNumber", user.PageNumber);
            //  parameters.Add("@PageSize", user.PageSize);


            var userList = await _dapperService.ReturnListAsync<ApplicantInfoList>("GetApplicantDetails", parameters);
            if (userList == null || !userList.Any())
            {
                throw new NotFoundException("Applicant data not found");
            }
            
            // Deserialize the JSON into the DegreeDetails list
            foreach (var applicant in userList)
            {
                if (!string.IsNullOrEmpty(applicant.DegreeDetails))
                {
                    // Deserialize the JSON into the DegreeDetails list
                    var degrees = JsonConvert.DeserializeObject<List<DegreeInfoList>>(applicant.DegreeDetails);
                    applicant.Degrees = degrees ?? new List<DegreeInfoList>();
                }
                else
                {
                    applicant.Degrees = new List<DegreeInfoList>();
                }
            }
            return userList.Adapt<List<ApplicantInfoList>>();
        }
            
        // Merit List Generation 

        public async Task GenerateMeritList(GenerateMeritListRequest request)
        {
            var session = await _context.Sessions
                .OrderBy(x => x.Id)
                .LastAsync();

            var meritLisNo = await _context.MeritLists
                .Where(x => x.SessionId == session.Id
                            && x.ProgramId == request.ProgramId
                            && x.ShiftId == request.ShiftId
                            && x.DepartmentId == request.DepartmentId)
                .CountAsync();

            DynamicParameters parameters = new();
            parameters.Add("@DepartmentId", request.DepartmentId);
            parameters.Add("@ProgramId", request.ProgramId);
            parameters.Add("@ShiftId", request.ShiftId);
            parameters.Add("@SessionId", session.Id);
            parameters.Add("@MeritListNo", meritLisNo+1);

            await _dapperService.ExecuteAsync("sp_GenerateMeritList", parameters);
        }


        public async Task<MeritListDataModel> GetMeritListData(int meritListId)
        {
            var meritLisDetails = await _context.MeritLists.
                Include(x => x.Department).
                Include(x => x.Program).
                Include(x => x.Shift).
                Include(x => x.Session)
                .Where(x => x.Id == meritListId)
                .Select(x => new MeritListDataModel
                {
                    Department = x.Department!.Name,
                    Program = x.Program!.Name,
                    Shift = x.Shift!.Name,
                    Session = x.Session!.Name!,
                })
                .FirstOrDefaultAsync();

            meritLisDetails!.MeritListDetails = await _context.MeritListDetails
                .Include(x => x.ApplicationForm)
                .ThenInclude(x => x.Applicant)
                .Where(x => x.MeritListId == meritListId)
                .Select(x => new MeritListDetailsModel
                {
                    FullName = x.ApplicationForm!.Applicant!.FullName,
                    FatherName = x.ApplicationForm!.Applicant!.FatherName!,
                    ApplicationId = x.ApplicationForm.Id
                })
                .ToListAsync();
            return meritLisDetails;
        }

        public async Task<PaginationResponse<MeritListResponse>>GetAllMeritListDetailsByFilter(LazyLoadEvent request,CancellationToken ct)
        {
            var result =            await _context.MeritLists.AsNoTracking()
                                            .Include(x => x.Department)
                                            .Include(x => x.Program)
                                            .Include(x => x.Shift)
                                            .Include(x => x.Session)
                                            .LazyFilters(request)
                                            .LazyOrderBy(request)
                                            .LazySkipTake(request)
                                            .Select(x => new MeritListResponse
                                            {
                                                Id = x.Id,
                                                Department = x.Department!.Name,
                                                Program = x.Program!.Name,
                                                MeritListNo = x.MeritListNo!.Value,
                                                Shift = x.Shift!.Name,
                                                Session = x.Session!.Name!,  
                                            }).ToListAsync(ct)
                                            .ConfigureAwait(false);

            return new PaginationResponse<MeritListResponse>
            {
                Data = result,
                Total = await _context.MeritLists.CountAsync(ct).ConfigureAwait(false)
            };

        }

        public async Task<List<Applicantmanagementresponse>> UpdateApplicantDetails(updateApplicantRequest request)
        {
            
            var user = await _context.ApplicationForms.AsNoTracking()
                .Where(x => x.ApplicantId == request.applicantId)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                if (string.Equals(request.VerificationStatusEid, "APPROVED", StringComparison.OrdinalIgnoreCase))
                {
                    user.VerificationStatusEid = (int)VerificationStatus.APPROVED;
                }
                else
                {                
                    user.VerificationStatusEid = (int)VerificationStatus.REJECTED;
                    user.Remarks = request.Remarks;
                }
                _context.ApplicationForms.Update(user);
                await _context.SaveChangesAsync();

                return user.Adapt<List<Applicantmanagementresponse>>();
            }
            return new List<Applicantmanagementresponse>();
        }

    }

}
