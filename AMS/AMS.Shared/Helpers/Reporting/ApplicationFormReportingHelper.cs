namespace AMS.SHARED.Helpers.Reporting
{
    public static class ApplicationFormReportingHelper
    {
        public const string FormId = "FormId";
        public const string ApplicantName = "ApplicantName";
        public const string AcademicSession = "AcademicSession";
        public const string FatherName = "FatherName";
        public const string Date = "Date";
        public const string ProfilePicture = "Text Box 21";
        public const string Cnic = "Cnic";
        public const string Male = "Male";
        public const string Female = "Female";
        public const string ApplicantMobileNo = "ApplicantMobileNo";
        public const string Dob = "Dob";
        public const string Domicile = "Domicile";
        public const string Province = "Province";
        public const string PostalAddress = "PostalAddress";
        public const string City = "City";
        public const string PostalCode = "PostalCode";
        public const string GuardianName = "GuardianName";
        public const string GuardianRelation = "GuardianRelation";
        public const string GuardianPhone = "GuardianPhone";
        public const string GuardianAddress = "GuardianAddress";

        public static string FormatCNIC(string input)
        {
            var cn = string.Join("".PadRight(2), input.ToCharArray());
            return string.Join("".PadRight(3), cn.Substring(0, 5 * 2 + 5), cn.Substring(5 * 2 + 5, 7 * 2 + 7), cn.Substring(12 * 2 + 12));
        }
        
        public static string FormatDOB(DateOnly date)
        {
            var input = date.ToString("ddMMyyyy");
            var cn = string.Join("".PadRight(2), input.ToCharArray());
            return string.Join("".PadRight(4), cn.Substring(0, 2 * 2 + 2), cn.Substring(2 * 2 + 2, 2 * 2 + 2), cn.Substring(4 * 2 + 2));
        }

        public static string FormatPhoneNo(string input) => input.Insert(4, "-");
        public static string ReplaceZeroWithEmpty(int input) => input == 0 ? "0" : input.ToString();
        public static string CreateAcademicSessionYearAndProgram(string academicSession, string AcademicYear, string programName) => $"ACADEMIC SESSION {academicSession} {AcademicYear} ${programName}";
        public static string NumberToString(int input) => input.ToString();
        public static string FormatCNICWithDashes(string input)
        {
            input = input.Insert(5, "-");
           return input.Insert(13, "-");
        }
        public static string GetTickOrCrossForMale(string input) => input.ToLower().Equals("male") ? "✔" : "✗";
        public static string GetTickOrCrossForFemale(string input) => input.ToLower().Equals("female") ? "✔" : "✗";
        public static string FormatSubmittedDate(DateTime inputDate) => $"Date : {inputDate.ToString("dd, MMM yyyy")}";
    }
}
