using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Program
{
    public record CreateProgramRequest(string Name,int ProgramTypeId);
    public record CreateProgramResponse(int Id, string Name,int ProgramTypeId);
}

