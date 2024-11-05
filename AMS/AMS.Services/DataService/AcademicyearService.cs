using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Academicyear;
using AMS.MODELS.Department;
using AMS.MODELS.Filters;
using AMS.MODELS.Shift;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.DataService
{
    public class AcademicyearService(AMSContext context) : IAcademicyearService
    {
        private readonly AMSContext _context = context;
    

        public async Task<List<AcademicyearResponse>> GetAllAcedamicYear(CancellationToken ct)
        {
            var acedamic = await _context.AcademicYears
         .Where(x => x.IsDeleted == false)
                                .ToListAsync()
                                .ConfigureAwait(false);

           
            return acedamic.Adapt<List<AcademicyearResponse>>();

        }

        public async Task<CreateAcademicyearResponse> CreateAcademicyear(CreateAcademicyearRequest Request,
     CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var entity = new AcademicYear
            {
                Name = Request.Name,
                StartDate=Request.StartDate,
                EndDate=Request.EndDate
            ,
            };
            await _context.AcademicYears.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Adapt<CreateAcademicyearResponse>();
        }

        public async Task<UpdateAcademicyearResponse> UpdateAcademicyear(UpdateAcademicyearRequest Request,
           CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(Request);
            var academic = await _context.AcademicYears.FindAsync(Request.Id) ?? throw new NotFoundException($"Academicyear doesn't exist with id: {Request.Id}");
            academic.Name = Request.Name;
            academic.StartDate= Request.StartDate;
            academic.EndDate= Request.EndDate;  
            await _context.SaveChangesAsync(ct);
            return academic.Adapt<UpdateAcademicyearResponse>();
        }

        public async Task DeleteAcademicYear(int id, CancellationToken ct)
        {
            var result = await _context.AcademicYears
                               .Where(x => x.Id == id).FirstOrDefaultAsync()
                                .ConfigureAwait(false);
            if (result is null)
            {

                throw new NotFoundException($"AcademicYears doesn't exist with id: {id}");
            }
            _context.AcademicYears.Remove(result);
            await _context.SaveChangesAsync(ct);


        }

        public async Task<PaginationResponse<AcademicyearResponse>> GetAcademicYearByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.AcademicYears.AsQueryable();
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
            return new PaginationResponse<AcademicyearResponse>
            {
                Data = result.Adapt<List<AcademicyearResponse>>(),
                Total = await query.CountAsync(ct),
            };
        }


    }
}
