using AMS.DATA;
using AMS.DOMAIN.Entities.Lookups;
using AMS.MODELS.Academicyear;
using AMS.MODELS.Department;
using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.ProgramType;
using AMS.MODELS.Session;
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
using static AMS.MODELS.Session.Session;

namespace AMS.SERVICES.DataService
{
    public class SessionService(AMSContext context) : ISessionService
    {
        private readonly AMSContext _context = context;

        public async Task<List<SessionResponse>> GetAllSession(CancellationToken ct)
        {
            var result = await _context.Sessions.Include(x=>x.AcademicYear)
                .Where(x => x.IsDeleted != true)
                                       .Select(x => new{
               x.Id,
               x.Name,
               x.StartDate,
               x.EndDate,
               x.AcademicYearId,
               AcademicYear=x.AcademicYear.Name

                                       })
                                       .ToListAsync(ct)
                                       .ConfigureAwait(false);
            return result.Adapt<List<SessionResponse>>();
        }   

            public async Task<CreateSessionResponse> CreateSession(CreateSessionRequest Request,
         CancellationToken ct)
            {
                ArgumentNullException.ThrowIfNull(Request);
                var entity = new AdmissionSession
                {
                    Name = Request.Name,
                    StartDate = Request.StartDate,
                    EndDate = Request.EndDate,
                    AcademicYearId=Request.AcademicYearId
                ,
                };
                await _context.Sessions.AddAsync(entity, ct);
                await _context.SaveChangesAsync(ct);
                return entity.Adapt<CreateSessionResponse>();
            }

            public async Task<UpdateSessionResponse> UpdateSession(UpdateSessionRequest Request,
               CancellationToken ct)
            {
                ArgumentNullException.ThrowIfNull(Request);
                var session = await _context.Sessions.FindAsync(Request.Id) ?? throw new NotFoundException($"Sesion doesn't exist with id: {Request.Id}");
            session.Name = Request.Name;
            session.StartDate = Request.StartDate;
            session.EndDate = Request.EndDate;
            session.AcademicYearId=Request.AcademicYearId;
                await _context.SaveChangesAsync(ct);
                return session.Adapt<UpdateSessionResponse>();
            }

            public async Task DeleteSession(int id, CancellationToken ct)
            {
                var result = await _context.Sessions
                                   .Where(x => x.Id == id).FirstOrDefaultAsync()
                                    .ConfigureAwait(false);
                if (result is null)
                {

                    throw new NotFoundException($"Session doesn't exist with id: {id}");
                }
                _context.Sessions.Remove(result);
                await _context.SaveChangesAsync(ct);

            }
        //public async Task<PaginationResponse<SessionResponse>> GetSessionByFilter(LazyLoadEvent request, CancellationToken ct)
        //{
        //    var query = _context.Sessions.AsQueryable();
        //    var result = string.IsNullOrWhiteSpace(request.GlobalFilter) ? await query.AsNoTracking()
        //                                    .LazyFilters(request)
        //                                    .LazyOrderBy(request)
        //                                    .LazySkipTake(request)
        //                                    .ToListAsync(ct)
        //                                    .ConfigureAwait(false)
        //    :
        //    await query.AsNoTracking()
        //        .LazySearch(request.GlobalFilter, "Name")
        //        .ToListAsync(ct)
        //        .ConfigureAwait(false);
        //    return new PaginationResponse<SessionResponse>
        //    {
        //        Data = result.Adapt<List<SessionResponse>>(),
        //        Total = await query.CountAsync(ct),
        //    };
        //}

        public async Task<PaginationResponse<SessionResponse>> GetSessionByFilter(LazyLoadEvent request, CancellationToken ct)
        {
            var query = _context.Sessions
                .Include(d => d.AcademicYear) // Include AcademicYear
                .Where(x => x.IsDeleted != true)
                .AsQueryable();

            var result = string.IsNullOrWhiteSpace(request.GlobalFilter)
                ? await query.AsNoTracking()
                    .LazyFilters(request)
                    .LazyOrderBy(request)
                    .LazySkipTake(request)
                    .Select(d => new SessionResponse(
                        d.Id,
                        d.Name,
                        (DateTime)d.StartDate,
                        (DateTime)d.EndDate,
                        (int)d.AcademicYearId,
                        d.AcademicYear.Name // Get Academic Year name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false)
                : await query.AsNoTracking()
                    .LazySearch(request.GlobalFilter, "Name")
                    .Select(d => new SessionResponse(
                        d.Id,
                        d.Name,
                        (DateTime)d.StartDate,
                        (DateTime)d.EndDate,
                        (int)d.AcademicYearId,
                        d.AcademicYear.Name
                    ))
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

            return new PaginationResponse<SessionResponse>
            {
                Data = result,
                Total = await query.CountAsync(ct)
            };
        }


    }
}

