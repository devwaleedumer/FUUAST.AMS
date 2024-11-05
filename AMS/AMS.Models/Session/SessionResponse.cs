using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Session
{
    public class Session
    {

        public record SessionResponse(int Id, string Name, DateTime StartDate, DateTime EndDate,int AcademicYearId, string AcademicYear);
    }

}
