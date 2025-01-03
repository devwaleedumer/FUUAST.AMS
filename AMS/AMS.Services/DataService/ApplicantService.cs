using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Identity;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.ApplicationForm.ApplicantDegree;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Enums.Shared;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace AMS.SERVICES.DataService
{
    public sealed class ApplicantService(AMSContext context, ICurrentUser currentUser, ILocalFileStorageService imageStorage) : IApplicantService
    {
        private readonly AMSContext _context = context;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly ILocalFileStorageService _imageStorage = imageStorage;
        #region Personal Details Methods
        public async Task<CreateApplicantPSInfoResponse> AddApplicantPersonalInformation(CreateApplicantPSInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var isCnicUnique = await _context.Applicants.AnyAsync((applicant) => applicant.Cnic == request.Cnic,cancellationToken);
            if (isCnicUnique) throw new BadRequestException("CNIC already exists"); 
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var user = await _context.Users.FindAsync(new object[] { userId },cancellationToken) ?? throw new NotFoundException("user not found");
            var applicant = request.Adapt<Applicant>();
            applicant.ApplicationUserId = userId;
            await _context.Applicants.AddAsync(applicant, cancellationToken);
            user.ProfilePictureUrl = await _imageStorage.UploadAsync<Applicant>(request.ImageRequest, FileType.Image, cancellationToken); ;
            await _context.SaveChangesAsync(cancellationToken);
            var response = applicant.Adapt<CreateApplicantPSInfoResponse>();
            response.ProfileImageUrl = user.ProfilePictureUrl;
            return response;
        }
        public async Task<ApplicantPSInfoResponse> GetApplicantPersonalInformation(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var user = await _context.Users.FindAsync(new object[] { userId },cancellationToken) ?? throw new NotFoundException("user not found");
            var applicant = await _context.Applicants
                                          .AsNoTracking()
                                          .Include((applicant) => applicant.Guardian)
                                          .Include((applicant) => applicant.EmergencyContact)
                                          .FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken)
                                          .ConfigureAwait(false)
                                           ?? throw new NotFoundException("applicant not found");
            var response = applicant.Adapt<ApplicantPSInfoResponse>();
            response.BloodGroup = applicant.BloodGroup ?? "";
            response.Gender = applicant.Gender ?? "";
            response.Religion = applicant.Religion ?? "";
            response.ProfilePictureUrl = user.ProfilePictureUrl ?? null;
            response.Dob = applicant.Dob;
            return response;
        }

        public async Task<UpdateApplicantPSInfoResponse> UpdateApplicantPersonalInformation(UpdateApplicantPSInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                
                var applicant = await _context.Applicants
                    .Include(applicant => applicant.ApplicationUser)
                    .Include(applicant => applicant.Guardian)
                    .Include(applicant => applicant.EmergencyContact)
                    .FirstOrDefaultAsync(applicant => applicant.Id == request.Id,cancellationToken);
                if (applicant is null)
                    throw new NotFoundException($"Applicant not found with id {request.Id}");
                //applicant 
                applicant.Dob = DateOnly.FromDateTime(request.Dob);
                applicant.BloodGroup = request.BloodGroup;
                applicant.Religion = request.Religion;
                applicant.Gender = request.Gender;
                applicant.City = request.City;
                applicant.Country = request.Country;
                applicant.Domicile = request.Domicile;
                applicant.Province = request.Province;
                applicant.FatherName = request.FatherName;
                applicant.MobileNo = request.MobileNo;
                applicant.PermanentAddress = request.PermanentAddress;
                applicant.PostalCode = request.PostalCode;
                // Guardian
                if (applicant.Guardian is null)
                {
                    var guardian = new Guardian
                    {
                        Name = request.Guardian.Name,
                        Relation = request.Guardian.Relation,
                        ContactNo = request.Guardian.ContactNo,
                        PermanentAddress = request.PermanentAddress
                    };
                    applicant.Guardian = guardian;
                }
                else
                {
                    applicant.Guardian.Relation = request.Guardian.Relation;
                    applicant.Guardian.ContactNo = request.Guardian.ContactNo;
                    applicant.Guardian.Name = request.Guardian.Name;
                    applicant.Guardian.PermanentAddress = request.PermanentAddress;
                }
                // Emergency Contact
                if (applicant.EmergencyContact is null)
                {
                    var emergencyContact = new EmergencyContact()
                    {
                        Name = request.EmergencyContact.Name,
                        Relation = request.EmergencyContact.Relation,
                        ContactNo = request.EmergencyContact.ContactNo,
                        PermanentAddress = request.Guardian.PermanentAddress
                    };
                    applicant.EmergencyContact = emergencyContact;
                }
                else
                {
                    applicant.EmergencyContact.Relation = request.EmergencyContact.Relation;
                    applicant.EmergencyContact.ContactNo = request.EmergencyContact.ContactNo;
                    applicant.EmergencyContact.Name = request.EmergencyContact.Name;
                    applicant.EmergencyContact.PermanentAddress = request.EmergencyContact.PermanentAddress;
                }
                if (request.ImageRequest is not null)
                {
                    await UpdateUserProfilePictureAsync(applicant.ApplicationUser!, request.ImageRequest, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                var response = applicant.Adapt<UpdateApplicantPSInfoResponse>();
                response.ProfilePictureUrl = applicant.ApplicationUser!.ProfilePictureUrl ?? "";
                return response;
            }
            catch 
            {

                await transaction.RollbackAsync(cancellationToken);
                throw new AMSException("Some thing went wrong, please try again later.");
            }
        }
        #endregion
        //Degrees 
        #region Degrees
        public async Task<List<CreateApplicantDegreeResponse>> AddApplicantDegrees(CreateApplicantDegreeListRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var newDegrees = request.Degrees.Adapt<List<ApplicantDegree>>();
            newDegrees.ForEach(degree => degree.ApplicantId = applicant.Id);
            await _context.ApplicantDegrees.AddRangeAsync(newDegrees, ct);

            await _context.SaveChangesAsync(ct);
            return newDegrees.Adapt<List<CreateApplicantDegreeResponse>>();
        }

        public async Task<List<EditApplicantDegreeResponse>> EditApplicantDegrees(EditApplicantDegreeListRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var degrees = await _context.ApplicantDegrees
                                        .Where(x => x.ApplicantId == applicant.Id)
                                        .ToListAsync(ct);
            foreach (var reqDegree in request.Degrees)
            {
                var degree = degrees.FirstOrDefault(d => d.Id == reqDegree.Id);
                if (degree != null)
                {
                    reqDegree.Adapt(degree);
                }
            }

            await _context.SaveChangesAsync(ct);
            return degrees.Adapt<List<EditApplicantDegreeResponse>>();
        }

        public async Task<List<ApplicantDegreeResponse>> GetApplicantDegrees(CancellationToken ct)
        {
            var applicant = await GetApplicantUtility(ct);
            var degrees = await _context.ApplicantDegrees
                                        .AsNoTracking()
                                        .Where(appDegrees => appDegrees.ApplicantId == applicant.Id)
                                        .ToListAsync(ct)
                                        .ConfigureAwait(false);
            return degrees.Adapt<List<ApplicantDegreeResponse>>();
        }

        #endregion
        //Private Methods
        private async Task<Applicant> GetApplicantUtility(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            var applicant = await _context.Applicants.FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken) ?? throw new NotFoundException("applicant not found"); ;
            return applicant;
        }  
        private async Task UpdateUserProfilePictureAsync(ApplicationUser user, FileRequest imageRequest, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                 _imageStorage.Remove(user.ProfilePictureUrl);
            }

            user.ProfilePictureUrl = await _imageStorage.UploadAsync<Applicant>(imageRequest, FileType.Image, cancellationToken);
        }
    }
}

