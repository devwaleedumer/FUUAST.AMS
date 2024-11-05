using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Academicyear
{
    public record UpdateAcademicyearRequest(int Id, string Name, DateTime StartDate, DateTime EndDate);
    public record UpdateAcademicyearResponse(int Id, string Name, DateTime StartDate, DateTime EndDate);
}
