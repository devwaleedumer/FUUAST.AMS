using AMS.MODELS.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.IDataService
{
    public interface IDashboardservice
    {
        Task<DashboardResponse> Dashboard();
    }
}