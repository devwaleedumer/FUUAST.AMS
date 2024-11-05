using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Program
{
    public record UpdateProgramRequest(int Id, string Name,int ProgramTypeId);
    public record UpdateProgramResponse(int Id, string Name, int ProgramTypeId);
}
