using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.Lookups;
namespace AMS.DOMAIN.Entities.AMS
{
    public class MeritList : IBaseEntity
    {
        public MeritList()
        {
            MeritListDetails = new HashSet<MeritListDetails>();
        }
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ProgramId { get; set; }
        public int DepartmentId { get; set; }
        public int ShiftId { get; set; }
        public int? MeritListNo { get; set; }

        public virtual AdmissionSession? Session { get; set; }
        public virtual Program? Program { get; set; }
        public virtual Department? Department { get; set; }
        public virtual TimeShift? Shift { get; set; }

        public virtual ICollection<MeritListDetails>? MeritListDetails { get; set; }
        public int? InsertedBy { get; set ; }
        public DateTime? InsertedDate { get ; set ; }
        public int? UpdatedBy { get ; set ; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
