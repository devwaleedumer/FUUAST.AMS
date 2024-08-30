

using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class DegreeLevel : IBaseEntity
    {
        public DegreeLevel()
        {
            DegreeGroups = new HashSet<DegreeGroup>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<DegreeGroup>? DegreeGroups { get; set; }
        public virtual ICollection<ApplicantDegree>? ApplicantDegrees { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
