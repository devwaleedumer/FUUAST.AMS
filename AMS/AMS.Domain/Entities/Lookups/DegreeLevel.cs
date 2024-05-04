

using AMS.DOMAIN.Base;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class DegreeLevel : IBaseEntity
    {
        public DegreeLevel()
        {
            DegreeTypes = new HashSet<DegreeType>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<DegreeType> DegreeTypes { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
