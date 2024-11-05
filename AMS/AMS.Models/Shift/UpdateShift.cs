using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Shift
{
    public record UpdateShiftRequest(int Id, string Name);
    public record UpdateShiftResponse(int Id, string Name);
}
