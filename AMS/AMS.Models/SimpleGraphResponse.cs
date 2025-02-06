using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS
{
    public class SimpleGraphResponse
    {
        public SimpleGraphResponse()
        {
            Data = new();
            Label = new();
        }
        public List<int>? Data { get; set; }
        public List<string>? Label { get; set; }
    }
}
