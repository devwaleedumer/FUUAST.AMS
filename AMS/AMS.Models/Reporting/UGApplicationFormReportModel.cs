using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Reporting
{
    public class UGApplicationFormReportModel
    {
        public UGApplicationFormReportModel()
        {
            AcademicRecordsTableModels = new();
            AppliedProgramTableModel = new();
        }
        public TextFieldsModel TextFields { get; set; } = default!;
        public ProfilePictureModel ProfilePictureModel { get; set; }
        public List<AcademicRecordsTableModel> AcademicRecordsTableModels { get; set; }
        public List<AppliedProgramTableModel> AppliedProgramTableModel { get; set; } = default!;
        public PersonalInformationTableModel PersonalInformationTableModel { get; set; } = default!;
        public EmergencyContactInformationTableModel EmergencyContactInformation { get; set; } = default!;
    }

    public class TextFieldsModel
    {
        public int FormId { get; set; } = default!;
        public string ApplicantName { get; set; } = default!;
        public string AcademicSession { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public string Date { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public string Male { get; set; } = default!;
        public string Female { get; set; } = default!;
        public string ApplicantMobileNo { get; set; } = default!;
        public DateOnly Dob { get; set; } = default!;
        public int PostalCode { get; set; } = default!;
        public string Domicile { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string PostalAddress { get; set; } = default!;
        public string City { get; set; } = default!;
        public string GuardianName { get; set; } = default!;
        public string GuardianRelation { get; set; } = default!;
        public string GuardianPhone { get; set; } = default!;
        public string GuardianAddress { get; set; } = default!;
    }

    public class AcademicRecordsTableModel
    {
        public string DegreeGroupName { get; set; } = default!;
        public string RollNo { get; set; } = default!;
        public string BoardOrUniversity { get; set; } = default!;
        public int PassingYear { get; set; } = default!;
        public int ObtainedMarks { get; set; } = default!;
        public int TotalMarks { get; set; } = default!;
    }
    public class AppliedProgramTableModel
    {
        public string DepartmentName { get; set; } = default!;
        public string Shift { get; set; } = default!;
    }
    public class PersonalInformationTableModel
    {
        public int FormNo { get; set; }
        public int VoucherId { get; set; }
        public string ApplicantName { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public string Cnic { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string MobileNo { get; set; } = default!;
        public string EverExpelled { get; set; } = default!;
        public string AcademicSessionYearAndProgram { get; set; } = default!;
    }
    public class EmergencyContactInformationTableModel
    {
        public string ContactName { get; set; } = default!;
        public string ContactRelation { get; set; } = default!;
        public string ContactPhone { get; set; } = default!;
        public string ContactAddress { get; set; } = default!;
    }

    public class ProfilePictureModel {
        public string ProfilePicture { get; set; } = default!;

    }
}
