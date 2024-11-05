using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Department
{
    public record CreateDepartmentRequest(string Name, int faculityId);
    public record CreateDepartmentResponse(int Id, string Name, int FaculityId);
}
