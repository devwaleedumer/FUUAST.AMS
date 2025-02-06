using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm.Applicant
{
    public class ApplicantInfoRequest
    {

       
        public string? UserName { get; set; }
      
        public string? Email { get; set; }
        public int VerificationStatusEid { get; set; }
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; } 
    }
    public class ApplicantInfoList
    {
        public int ApplicantId { get; set; }
        public string? UserName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public string ?DOB { get; set; }
        public string? Cnic { get; set; }
        public int ApplicationUserId { get; set; }
        public bool IsActive { get; set; }
        public int ApplicationFormId { get; set; }
        public int FeeChalanId {  get; set; }
        public int FeeChalanSubmissionId {  get; set; }
        public string DocumentUrl {  get; set; }
        public bool IsSubmitted { get; set; }
        public int VerificationStatusEid { get; set; }
        public int ProgramId {  get; set; }
        public string? ProgramName {  get; set; }
        public int ProgramTypeId {  get; set; }
        public string? ProgramTypeName {  get; set; }
        public bool IsDeleted { get; set; }
        public string? DegreeDetails { get; set; }

        // List to hold multiple degrees for an applicant
        public List<DegreeInfoList> Degrees { get; set; } = new List<DegreeInfoList>();
    //    public int TotalRecords { get; set; }
     //   public int TotalPages { get; set; }
    }

    public class DegreeInfoList
    {
        public int ApplicantDegreeId { get; set; }
        public string? BoardOrUniversityName { get; set; }  // Changed to PascalCase
        public int PassingYear { get; set; }               // Changed to PascalCase
        public string? Subject { get; set; }
        public string? RollNo { get; set; }                    // Changed to PascalCase
        public int TotalMarks { get; set; }
        public int ObtainedMarks { get; set; }
        public int DegreeId { get; set; }
        public string? DegreeName { get; set; }
        public int DegreeLevelId { get; set; }
        public string? DegreeLevelName { get; set; }        // Changed from Name to DegreeLevelName for clarity
    }


    public class updateApplicantRequest
    { 
        public int applicantId {  get; set; }
        public string VerificationStatusEid {  get; set; }
        public string Remarks { get; set; }
    }
   
}
