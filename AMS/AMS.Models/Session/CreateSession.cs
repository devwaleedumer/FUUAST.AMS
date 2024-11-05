using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Session
{
    public record CreateSessionRequest(string Name, DateTime StartDate, DateTime EndDate,int AcademicYearId);
    public record CreateSessionResponse(int Id, string Name, DateTime StartDate, DateTime EndDate,int AcademicYearId);
}
