using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.MeritList
{
    public class MeritListDataModel
    {
        public MeritListDataModel()
        {
            MeritListDetails = new List<MeritListDetailsModel>();
        }
        public string Program{ get; set; }
        public string Department{ get; set; }
        public string Shift{ get; set; }
        public string Session{ get; set; }
        public string MeritListNo{ get; set; }
        public List<MeritListDetailsModel> MeritListDetails { get; set; }
    }
}
