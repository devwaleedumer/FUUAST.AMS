using AMS.MODELS;
using AMS.MODELS.Dashboard;
using AMS.MODELS.Filters;
using System.Threading.Tasks;


namespace AMS.SERVICES.IDataService
{
    public interface IDashboardservice
    {
        Task<DashboardResponse> Dashboard();
        Task<SimpleGraphResponse> FeeRevenueData();
        Task<SimpleGraphResponse> ApplicationsByDepartmentData();
        Task<PaginationResponse<AuditResponse>> AuditData(LazyLoadEvent request, CancellationToken ct);
        }
}