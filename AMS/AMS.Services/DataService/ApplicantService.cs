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
    public sealed class ApplicantService(AMSContext _context, ICurrentUser _currentUser, ILocalFileStorageService _imageStorage) : IApplicantService
    {
        #region Personal Details Methods
        public async Task<CreateApplicantPSInfoResponse> AddApplicantPersonalInformation(CreateApplicantPSInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var user = await _context.Users.FindAsync(userId) ?? throw new NotFoundException("user not found");
            var applicant = request.Adapt<Applicant>();
            user.ProfilePictureUrl = await _imageStorage.UploadAsync<Applicant>(request.ImageRequest, FileType.Image, cancellationToken); ;
            applicant.ApplicationUserId = userId;
            await _context.Applicants.AddAsync(applicant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = applicant.Adapt<CreateApplicantPSInfoResponse>();
            response.ProfileImageUrl = user.ProfilePictureUrl;
            return response;
        }
        public async Task<ApplicantPSInfoResponse> GetApplicantPersonalInformation(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var user = await _context.Users.FindAsync(userId, cancellationToken) ?? throw new NotFoundException("user not found");
            var applicant = await _context.Applicants
                                          .AsNoTracking()
                                          .Include((applicant) => applicant.Guardian)
                                          .Include((applicant) => applicant.EmergencyContact)
                                          .FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken)
                                          .ConfigureAwait(false)
                                           ?? throw new NotFoundException("applicant not found");
            var response = applicant.Adapt<ApplicantPSInfoResponse>();
            response.ProfilePictureUrl = user.ProfilePictureUrl ?? "";
            response.Dob = applicant.Dob.Date;
            return response;
        }

        public async Task<UpdateApplicantPSInfoResponse> UpdateApplicantPersonalInformation(UpdateApplicantPSInfoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var applicant = await GetApplicantUtility(cancellationToken);
                if (applicant.Id != request.Id) throw new BadRequestException("Invalid request please try again");
                var user = await _context.Users.FindAsync(applicant.ApplicationUserId, cancellationToken) ?? throw new NotFoundException("user not found");
                request.Adapt(applicant);
                if (request.ImageRequest is not null)
                {
                   await UpdateUserProfilePictureAsync(user, request.ImageRequest, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                var response = applicant.Adapt<UpdateApplicantPSInfoResponse>();
                response.ProfilePictureUrl = user.ProfilePictureUrl ?? "";
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
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
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

