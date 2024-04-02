using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Entities.Lookups
{
    public class AcademicYear
    {
        public AcademicYear()
        {
            Sessions = new HashSet<Session>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
