using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Entities.Lookups;
using AMS.DOMAIN.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace AMS.DOMAIN.Entities.AMS
{
    public class Applicant : IBaseEntity

    {
        // Applicant Contains only personal Information
        public Applicant()
        {
            this.Degrees = new HashSet<ApplicantDegree>();
        }
        public int Id { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public DateTime Dob { get; set; }
        public string MobileNo { get; set; }
        public string Domicile { get; set; }
        public string Province { get; set; }

        // Required for UG and Optional for PG
        public int? PostalCode { get; set; }
        public string PermanentAddress { get; set; }
        // Required for PG and Optional for UG
        public string? PostalAddress { get; set; }
        // Required for PG and Optional for UG
        public string? EmploymentDetails { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HeardAboutUniFrom { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public required string Religion { get; set; }
        public required string Gender { get; set; }
        public required string BloodGroup { get; set; }

        public virtual Guardian? Guardian { get; set; }
        public virtual EmergencyContact? ContactInfo { get; set; }
        public virtual ApplicationForm? ApplicationForm { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual EntranceTestDetail? EntranceTestDetail { get; set; }
        public virtual ICollection<ApplicantDegree>? Degrees { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

