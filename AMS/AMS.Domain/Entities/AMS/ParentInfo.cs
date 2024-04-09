using AMS.DOMAIN.Base;


namespace AMS.DOMAIN.Entities.AMS
{
    public class ParentInfo : IBaseEntity
    {
        public int Id { get; set; }
        public required string FatherName { get; set; }
        public required string MotherName { get; set; }
        public string FatherContact { get; set; }
        public string FatherOccupation { get; set; }
        public required string FatherCNIC { get; set; }
        public bool IsFatherDeceased { get; set; }
        public int ApplicantId { get; set; }

        public Applicant Applicant { get; set; }


        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }

}
