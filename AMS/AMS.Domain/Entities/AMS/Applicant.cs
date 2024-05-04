using AMS.DOMAIN.Base;
using AMS.DOMAIN.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace AMS.DOMAIN.Entities.AMS
{
    public class Applicant : IBaseEntity

    {
        public Applicant()
        {
            this.Degrees = new HashSet<ApplicantDegree>();
            this.Addresses = new HashSet<Address>();
        }
        public int Id { get; set; }
        public string? Cnic { get; set; }
        public bool? IsDisabled { get; set; }
        public string? DisablitityDetails { get; set; }
        public DateTime Dob { get; set; }
        public required string WhatsappNo { get; set; }
        public string? NextOfKinName { get; set; }
        public string? NextOfKinRelation { get; set; }
        public required string DomicileDistrict { get; set; }
        public required string DomicileProvince { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public int ApplicationFormId { get; set; }
        public required string Religion { get; set; }
        public required string Gender { get; set; }
        public required string BloodGroup { get; set; }

        public virtual Guardian? Guardian { get; set; }
        public virtual ParentInfo? ParentInfo { get; set; }
        public virtual EmergencyContact? ContactInfo { get; set; }
        public virtual ApplicationForm? ApplicationForm { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual ICollection<ApplicantDegree>? Degrees { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
