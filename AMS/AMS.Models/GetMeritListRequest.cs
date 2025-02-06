using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS
{
    public class GetMeritListRequest
    {
        public int ShiftId { get; set; }
        public int DepartmentId { get; set; }
        public int ProgramId { get; set; }
        public int MeritListNo { get; set; }
        public int SessionId { get; set; }
    }
}
