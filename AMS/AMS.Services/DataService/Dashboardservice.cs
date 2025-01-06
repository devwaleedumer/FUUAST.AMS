using AMS.DATA;
using AMS.DOMAIN.Identity;
using AMS.MODELS.Dashboard;
using AMS.SERVICES.IDataService;
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


      public async Task<DashboardResponse> Dashboard()
        {
            var applicant = await _context.Applicants.CountAsync();
             var user = await _context.Users.CountAsync();
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
