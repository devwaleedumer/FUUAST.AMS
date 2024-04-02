using FUUAST.AMS.DOMAIN.Identity;


namespace FUUAST.AMS.DOMAIN.Entities.AMS
{
    public class Applicant : ApplicationUser

    {
        public Applicant()
        {
            this.Degrees = new HashSet<Degree>();
            this.Addresses = new HashSet<Address>();
        }
        public string? Cnic { get; set; }
        public bool? IsDisabled { get; set; }
        public string? DisablitityDetails { get; set; }
        public DateTime Dob { get; set; }
        public string MobileNo { get; set; }
        public string WhatsappNo { get; set; }
        public string? NextOfKinName { get; set; }
        public string? NextOfKinRelation { get; set; }
        public string DomicileDistrict { get; set; }
        public string DomicileProvince { get; set; }

        public string Religion { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }

        public virtual Guardian? Guardian { get; set; }
        public virtual ParentInfo? ParentInfo { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual EmergencyContact? ContactInfo { get; set; }
        public virtual ICollection<Degree> Degrees { get; set; }
    }
}
