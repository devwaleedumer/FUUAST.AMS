using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.MODELS;
using AMS.MODELS.Dashboard;
using AMS.MODELS.Filters;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.DataService
{
    public class Dashboardservice:IDashboardservice
    {
        private readonly AMSContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public Dashboardservice(  AMSContext context, UserManager<ApplicationUser> userManager) {
            _context= context;
            _userManager= userManager;
        }

        public async Task<SimpleGraphResponse> FeeRevenueData() {
            
            var response = await (from fc in _context.FeeChallans
                            join af in _context.ApplicationForms on fc.ApplicationFormId equals af.Id
                            join fcd in _context.FeeChallanSubmissionDetails on fc.Id equals fcd.FeeChallanId
                            where af.VerificationStatusEid == 2
                            group fcd by fcd.InsertedDate.Value.Date into g
                            select new 
                            {
                                Label = g.Key.Date,
                                Data = g.Sum(x => x.FeeChallan!.TotalFee)
                            }).ToListAsync();

            return new SimpleGraphResponse
            {
                Data = response.Select(x => x.Data).ToList(),
                Label = response.Select(x => x.Label.ToString("dd MMM")).ToList()
            };

        }

        public async Task<SimpleGraphResponse> ApplicationsByDepartmentData()
        {

            var response = await (from form in _context.ApplicationForms
                                  join pa in _context.ProgramsApplied on form.Id equals pa.ApplicationFormId
                                  join d in _context.Departments.DefaultIfEmpty() on pa.DepartmentId equals d.Id
                                  group d by d.Name into g
                                  select new
                                  {
                                      Label = g.Key,
                                      Data = g.Count()
                                  }).ToListAsync();
            return new SimpleGraphResponse
            {
                Data = response.Select(x => x.Data).ToList(),
            
                
                Label = response.Select(x => x.Label).ToList()
            };
        }

        public async Task<PaginationResponse<AuditResponse>> AuditData(LazyLoadEvent request,CancellationToken ct)
        {
             var result = await _context.AuditTrails
                .AsNoTracking()
                .LazyFilters(request)
                .LazyOrderBy(request)
                .LazySkipTake(request)
                .Select(x => new AuditResponse
            {
                 Id = x.Id,
                 DateTime = x.DateTime,
                 TableName = x.TableName,
                 Type = x.Type,
                 UserId = x.UserId
            }).ToListAsync(ct).ConfigureAwait(false);
            return new PaginationResponse<AuditResponse>
            {
                Data = result,
                Total = await _context.AuditTrails.CountAsync(ct).ConfigureAwait(false)
            };
            
        }

        public async Task<DashboardResponse> Dashboard()
        {
            var applicant = await _context.Applicants.CountAsync();
             var user = await _context.Users.Where(x => x.UserTypeEid ==1).CountAsync();
           var approved = await _context.ApplicationForms
            .Where(x => x.VerificationStatusEid == 2)
            .CountAsync();
            var submittedapplication= await _context.ApplicationForms
                .Where(x=> x.IsSubmitted == true)
                .CountAsync();


            return new DashboardResponse
            {
                TotalApplicant = applicant,
                TotalUser = user,
                ApprovedApplication= approved,
                SubmittedApplication=submittedapplication

            };
        }

    }
}
