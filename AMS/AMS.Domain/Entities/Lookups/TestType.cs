using AMS.DOMAIN.Base;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class TestType : IBaseEntity
    {
        public TestType()
        {
            EntranceTestDetails =  new HashSet<EntranceTestDetail>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }


        public virtual ICollection<EntranceTestDetail>? EntranceTestDetails { get; set; }
    }
}
