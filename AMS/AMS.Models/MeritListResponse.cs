using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS
{
    public class MeritListResponse
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string Program { get; set; }
        public string Shift { get; set; }
        public string Session { get; set; }
        public int MeritListNo { get; set; }
    }
}
