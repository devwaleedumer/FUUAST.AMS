using AMS.DOMAIN.Base;


namespace AMS.DOMAIN.Entities.Lookups
{
    public class ProgramType: IBaseEntity
    {
        public ProgramType()
        {
            Programs = new HashSet<Program>();
        }
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Program> Programs { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
