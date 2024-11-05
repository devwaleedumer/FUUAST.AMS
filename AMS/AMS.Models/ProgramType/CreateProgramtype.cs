using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ProgramType
{
    public record CreateProgramTypeRequest(string Name);
    public record CreateProgramTypeResponse(int Id, string Name);
}
