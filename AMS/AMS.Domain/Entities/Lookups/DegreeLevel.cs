

using AMS.DOMAIN.Base;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class DegreeLevel : IBaseEntity
    {
        public DegreeLevel()
        {
            PreviousDegreeDetails = new HashSet<PreviousDegreeDetail>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<PreviousDegreeDetail> PreviousDegreeDetails { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
