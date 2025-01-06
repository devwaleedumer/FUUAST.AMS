using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Entities.Lookups;
using AMS.DOMAIN.Identity;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Program;
using AMS.MODELS.Shift;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Enums.AMS;
using AMS.SHARED.Enums.Shared;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Management.Automation;
using System.Threading;

namespace AMS.SERVICES.DataService
{
    public class ShiftService(AMSContext context, ILocalFileStorageService imageStorage, IWebHostEnvironment hostingEnvironment) : IShiftService
    {
        private readonly AMSContext _context = context;
       private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
        private readonly ILocalFileStorageService _imageStorage = imageStorage;
        public async Task<List<ShiftResponse>> GetTimeShiftByDepartmentAndProgramId(int departmentId, int programId, CancellationToken ct)
        {
            var result = await _context.ProgramDepartments
                                       .AsNoTracking()
                                       .Where(pd => pd.DepartmentId == departmentId && pd.ProgramId == programId)
                                       .Select(x => x.TimeShift)
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<ShiftResponse>>();
        }

        public async Task <List<ShiftResponse>> GetAllShift(CancellationToken ct)
        {
            var result = await _context.TimeShifts
                                .AsNoTracking().Where(x=>x.IsDeleted==false)
                                .ToListAsync()
                                .ConfigureAwait(false);
            return result.Adapt<List<ShiftResponse>>();
        }

        public async Task<CreateShiftResponse> CreateFaculty(CreateShiftRequest shiftRequest,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(shiftRequest);
            var entity = new TimeShift 
            { 
                Name = shiftRequest.Name,
                Description=shiftRequest.Description,
            };
            await _context.TimeShifts.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateShiftResponse>();
        }

        public async Task<UpdateShiftResponse> UpdateShift(UpdateShiftRequest shiftRequest,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(shiftRequest);
            var shift = await _context.TimeShifts.FindAsync(shiftRequest.Id) ?? throw new NotFoundException($"shift doesn't exist with id: {shiftRequest.Id}");
            shift.Name = shiftRequest.Name;
            await _context.SaveChangesAsync(ct);
            return shift.Adapt<UpdateShiftResponse>();
        }

        public async Task DeleteShift(int id,CancellationToken ct)
        {
            var result = await _context.TimeShifts
                               .Where(x => x.Id ==id ).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"Faculty doesn't exist with id: {id}");
            }
            _context.TimeShifts.Remove(result);
            await _context.SaveChangesAsync(ct);
           
        }


        //public async Task<Imageresponse> AddFeeImage(IFormFile file, CancellationToken ct)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        throw new ArgumentException("No file uploaded.");
        //    }

        //    try
        //    {
        //        // Check for cancellation before processing
        //        ct.ThrowIfCancellationRequested();

        //        // Generate a unique file name using GUID to avoid collisions
        //        var fileInfo = new FileInfo(file.FileName);
        //        var newFileName = "Image_" + Guid.NewGuid().ToString() + fileInfo.Extension;

        //        // Define the path to save the file
        //        var path = Path.Combine(_hostingEnvironment.WebRootPath, "Images", newFileName);

        //        // Create the directory if it doesn't exist
        //        var directoryPath = Path.GetDirectoryName(path);
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            Directory.CreateDirectory(directoryPath);
        //        }

        //        // Save the file to the server
        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream, ct); // Pass ct to allow cancellation during the copy
        //        }

        //        // Create the FeeChallanSubmissionDetail entity
        //        var feeSubmission = new FeeChallanSubmissionDetail
        //        {
        //            DocumentUrl = "/Images/" + newFileName ,// Assigning the URL of the uploaded image
        //            BranchNameWithCity = "avc",
        //            FeeChallanId=1
        //        };

        //        // Save the FeeSubmissionDetail to the database
        //        _context.FeeChallanSubmissionDetails.Add(feeSubmission);
        //        await _context.SaveChangesAsync(ct); // Pass ct to support cancellation during DB save

        //        // Return the adapted result
        //        return feeSubmission.Adapt<Imageresponse>();
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        // Handle the cancellation scenario
        //        throw new TaskCanceledException("The file upload was cancelled.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle other exceptions as needed
        //        throw new Exception("An error occurred while uploading the file.", ex);
        //    }
        //}

        public async Task<PaginationResponse<ShiftResponse>> GetShiftByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.TimeShifts.AsQueryable();
            var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
                                            .LazyFilters(request)
                                            .LazyOrderBy(request)
                                            .LazySkipTake(request)
                                            .ToListAsync(ct)
                                            .ConfigureAwait(false)
            :
            await query.AsNoTracking()
                .LazySearch(request.GlobalFilter, "Name")
                .ToListAsync(ct)
                .ConfigureAwait(false);
            return new PaginationResponse<ShiftResponse>
            {
                Data = result.Adapt<List<ShiftResponse>>(),
                Total = await query.CountAsync(ct),
            };
        }
    }
    }
