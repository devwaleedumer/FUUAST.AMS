using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Shift
{
    public record CreateShiftRequest(string Name,string Description);
    public record CreateShiftResponse(int Id, string Name);
}
