using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class DegreeType : IBaseEntity
    {
        public DegreeType()
        {
            ApplicantDegrees = new HashSet<ApplicantDegree>();
        }
        public int Id { get; set; }
        public required int DegreeLevelId { get; set; }
        public required string DegreeName { get; set; }
        public virtual DegreeLevel? DegreeLevel { get; set; }
        public virtual ICollection<ApplicantDegree>? ApplicantDegrees { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
