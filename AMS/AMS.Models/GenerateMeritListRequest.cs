using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS
{
    public class GenerateMeritListRequest
    {
        public int ShiftId { get; set; }
        public int DepartmentId { get; set; }
        public int ProgramId { get; set; }
    }
}
