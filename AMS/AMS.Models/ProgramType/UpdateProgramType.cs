using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ProgramType
{
    public record UpdateProgramTypeRequest(int Id, string Name);
    public record UpdateProgramTypeResponse(int Id, string Name);
}
