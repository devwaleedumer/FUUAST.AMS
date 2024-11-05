using AMS.MODELS.Faculity;
using AMS.MODELS.Filters;
using AMS.MODELS.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AMS.MODELS.Session.Session;

namespace AMS.SERVICES.IDataService
{
    public interface ISessionService
    {
        Task<List<SessionResponse>> GetAllSession(CancellationToken ct);
        Task<CreateSessionResponse> CreateSession(CreateSessionRequest Request,
         CancellationToken ct);
        Task<UpdateSessionResponse> UpdateSession(UpdateSessionRequest Request,
               CancellationToken ct);
        Task DeleteSession(int id, CancellationToken ct);
        Task<PaginationResponse<SessionResponse>> GetSessionByFilter(LazyLoadEvent request, CancellationToken ct);
    }
}
