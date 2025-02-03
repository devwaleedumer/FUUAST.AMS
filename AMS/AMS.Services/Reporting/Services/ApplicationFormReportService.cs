using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.SERVICES.Reporting.IService;
using AMS.SHARED.Exceptions;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using AMS.MODELS.Reporting;
using AMS.SHARED.Helpers.Reporting;
using Microsoft.AspNetCore.Hosting;

namespace AMS.SERVICES.Reporting.Services
{
    public class ApplicationFormReportService(IWordReportGenerator wordReportGenerator, AMSContext context, IWebHostEnvironment env) : IApplicationFormReportService
    {
        private readonly IWordReportGenerator _wordReportGenerator = wordReportGenerator;
        private readonly AMSContext _context = context;
        private readonly IWebHostEnvironment _env = env;
        public async Task<byte[]> GenerateUGApplicationFormPdfReportAsync(int applicationFormId)
        {
            var data = await GetApplicationFormUgData(applicationFormId);
            //get directory of reports
            var reportFileDirectory = Path.Combine(_env.ContentRootPath, "Reports");
            // get path of template docx file
            var reportTemplatePath = Path.Combine(reportFileDirectory, "ug_admission_template.docx");
            // name of new file without extension
            var newFileName = $"UG-{data.PersonalInformationTableModel!.ApplicantName}-Admission Form-{data.TextFields.FormId}";
            // new docx file path
            var newFilePathDocx = Path.Combine(reportFileDirectory, string.Join(".", newFileName, "docx"));
            // new pdf file path for cleanup purpose
            //var newFilePathPdf = Path.Combine(reportFileDirectory, string.Join(".", newFileName, "pdf"));
            try
            {
                // copy template into new path
                File.Copy(reportTemplatePath, newFilePathDocx);
                // process new docx
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newFilePathDocx, true))
                {
                    // Process Academic degrees table
                    // Process Department and shift degrees table
                    // Process Personal Information table
                    // Process Emergency Contact Information table
                    ReplaceUGTables(wordDoc, data.AcademicRecordsTableModels, data.AppliedProgramTableModel, data.PersonalInformationTableModel, data.EmergencyContactInformation);
                    // Process All TextBoxes
                    ReplaceAllTextBoxes(wordDoc, MapTextFieldsDataToKeyValue(data.TextFields));
                    // Process Picture Box
                    ReplacePictureBox(ApplicationFormReportingHelper.ProfilePicture, data.ProfilePictureModel.ProfilePicture, wordDoc);
                    // save document
                    wordDoc.MainDocumentPart!.Document.Save();
                }
                var bytes = await _wordReportGenerator.ConvertWordToPdfAsync(newFilePathDocx);
                return bytes;
            }
            finally
            {
                if (File.Exists(newFilePathDocx))
                    File.Delete(newFilePathDocx);
            }
        }
        private void ReplaceAllTextBoxes(WordprocessingDocument doc, List<KeyValuePair<string, string>> data)
        {
            var mainPart = doc.MainDocumentPart;

            foreach (var kvp in data)
            {
                string textBoxId = kvp.Key; // Alt Text Title of the text box
                string newText = kvp.Value; // Replacement text

                // Locate the drawing element for the text box by Alt Text Title
                var drawing = mainPart!.Document.Body!
                    .Descendants<Drawing>()
                    .FirstOrDefault(d => d.Descendants<DocProperties>().FirstOrDefault()?.Description == textBoxId);

                if (drawing != null)
                {
                    // Locate the text content inside the text box
                    var textElements = drawing.Descendants<Text>().ToList();

                    if (textElements.Any())
                    {
                        // Clear existing text
                        foreach (var textElement in textElements)
                        {
                            textElement.Text = string.Empty;
                        }
                    }

                    // Insert new text
                    var run = drawing.Descendants<Run>().FirstOrDefault();
                    if (run != null)
                    {
                        run.Append(new Text(newText));
                    }

                }

            }
        }

        private async Task<UGApplicationFormReportModel> GetApplicationFormUgData(int applicationFormId)
        {
            // TODO: check for fee challan submitted or not
            if (!(await _context.ApplicationForms.AnyAsync(x => x.Id == applicationFormId && x.IsSubmitted == true)))
            {
                throw new BadRequestException("Please submit form properly then try to generate form.");
            }
            var applicationForm = await _context.ApplicationForms
                .AsNoTracking()
                .Include(app => app.ProgramsApplied!)
                .ThenInclude(ap => ap.TimeShift)
                .Include(app => app.ProgramsApplied!)
                    .ThenInclude(ap => ap.Department)
                .Include(app => app.Session)
                .Include(app => app.Session)
                .ThenInclude(session => session!.AcademicYear)
                .Include(app => app.FeeChallan)
                .Include(app => app.Applicant)
                .Include(app => app.Program)
                .Include(app => app.Applicant!.Degrees!)
                    .ThenInclude(dg => dg.DegreeGroup)
                .Include(app => app.Applicant!.EmergencyContact)
                .Include(app => app.Applicant!.Guardian)
                .Include(app => app.Applicant!.ApplicationUser)
                .Include(app => app.FeeChallan)
                .FirstOrDefaultAsync(app => app.Id == applicationFormId)
                .ConfigureAwait(false);

            return PerformModelMapping(applicationForm!);

        }
        private void ReplaceUGTables(WordprocessingDocument mainDoc,
            List<AcademicRecordsTableModel> academicRecordTableData,
            List<AppliedProgramTableModel> appliedProgramTableData,
            PersonalInformationTableModel personalInformationTableData,
            EmergencyContactInformationTableModel emergencyContactInformationData)
        {
            // Locate the first table (you can add logic to find specific tables)
            var tables = mainDoc.MainDocumentPart!.Document.Body!.Descendants<Table>().ToList();
            var academicRecordTable = tables[0];
            var appliedProgramsTable = tables[1];
            var personalInformationTable = tables[2];
            var emergencyContactTable = tables[3];
            Parallel.Invoke(
                () => ReplaceAcademicRecordTable(academicRecordTable, academicRecordTableData),
                () => ReplaceAppliedProgramsTable(appliedProgramsTable, appliedProgramTableData),
                () => ReplacePersonalInformationTable(personalInformationTable, personalInformationTableData),
                () => ReplaceEmergencyContactNo(emergencyContactTable, emergencyContactInformationData)
                );
        }
        private void ReplaceAcademicRecordTable(Table table, List<AcademicRecordsTableModel> data)
        {
            var rows = table.Elements<TableRow>().ToList();
            // Matric
            var matricCells = rows[0].Elements<TableCell>().ToList();
            AddTextToCell(matricCells[0], data[0].DegreeGroupName);
            AddTextToCell(matricCells[1], data[0].RollNo);
            AddTextToCell(matricCells[2], data[0].BoardOrUniversity);
            AddTextToCell(matricCells[3], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[0].PassingYear));
            AddTextToCell(matricCells[4], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[0].ObtainedMarks));
            AddTextToCell(matricCells[5], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[0].TotalMarks));

            // Intermediate
            var intermediateCells = rows[1].Elements<TableCell>().ToList();
            AddTextToCell(intermediateCells[0], data[1].DegreeGroupName);
            AddTextToCell(intermediateCells[1], data[1].RollNo);
            AddTextToCell(intermediateCells[2], data[1].BoardOrUniversity);
            AddTextToCell(intermediateCells[3], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[1].PassingYear));
            AddTextToCell(intermediateCells[4], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[1].ObtainedMarks));
            AddTextToCell(intermediateCells[5], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[1].TotalMarks));
            if (data.Count == 2)
            {
                var fourth = rows[3].Elements<TableCell>().ToList();
                AddTextToCell(fourth[0], "");
                AddTextToCell(fourth[1], "");
                AddTextToCell(fourth[2], "");
                AddTextToCell(fourth[3], "");
                AddTextToCell(fourth[4], "");
                AddTextToCell(fourth[5], "");

                var third = rows[2].Elements<TableCell>().ToList();
                AddTextToCell(third[0], "");
                AddTextToCell(third[1], "");
                AddTextToCell(third[2], "");
                AddTextToCell(third[3], "");
                AddTextToCell(third[4], "");
                AddTextToCell(third[5], "");
            }
            if (data.Count == 3)
            {
                // Other
                var otherCells = rows[3].Elements<TableCell>().ToList();
                AddTextToCell(otherCells[0], data[2].DegreeGroupName);
                AddTextToCell(otherCells[1], data[2].RollNo);
                AddTextToCell(otherCells[2], data[2].BoardOrUniversity);
                AddTextToCell(otherCells[3], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].PassingYear));
                AddTextToCell(otherCells[4], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].ObtainedMarks));
                AddTextToCell(otherCells[5], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].TotalMarks));

                // fourth
                var fourth = rows[2].Elements<TableCell>().ToList();
                AddTextToCell(fourth[0], "");
                AddTextToCell(fourth[1], "");
                AddTextToCell(fourth[2], "");
                AddTextToCell(fourth[3], "");
                AddTextToCell(fourth[4], "");
                AddTextToCell(fourth[5], "");
            }
            if (data.Count == 4)
            {

                var thirdCells = rows[3].Elements<TableCell>().ToList();
                AddTextToCell(thirdCells[0], data[2].DegreeGroupName);
                AddTextToCell(thirdCells[1], data[2].RollNo);
                AddTextToCell(thirdCells[2], data[2].BoardOrUniversity);
                AddTextToCell(thirdCells[3], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].PassingYear));
                AddTextToCell(thirdCells[4], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].ObtainedMarks));
                AddTextToCell(thirdCells[5], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[2].TotalMarks));


                var otherCells = rows[2].Elements<TableCell>().ToList();
                AddTextToCell(otherCells[0], data[3].DegreeGroupName);
                AddTextToCell(otherCells[1], data[3].RollNo);
                AddTextToCell(otherCells[2], data[3].BoardOrUniversity);
                AddTextToCell(otherCells[3], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[3].PassingYear));
                AddTextToCell(otherCells[4], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[3].ObtainedMarks));
                AddTextToCell(otherCells[5], ApplicationFormReportingHelper.ReplaceZeroWithEmpty(data[3].TotalMarks));


            }
        }
        private void ReplacePersonalInformationTable(Table table, PersonalInformationTableModel data)
        {
            var rows = table.Elements<TableRow>().ToList();

            //row 0 contains only 1 element
            var titleCells = rows[0].Elements<TableCell>().ToList();
            AddTextToCell(titleCells[0], data.AcademicSessionYearAndProgram);
            //escaping row2
            var formNoCells = rows[2].Elements<TableCell>().ToList();
            AddTextToCell(formNoCells[1], ApplicationFormReportingHelper.NumberToString(data.FormNo));
            var voucherNoCells = rows[3].Elements<TableCell>().ToList();
            AddTextToCell(voucherNoCells[1], ApplicationFormReportingHelper.NumberToString(data.VoucherId));
            var fullNameCells = rows[4].Elements<TableCell>().ToList();
            AddTextToCell(fullNameCells[1], data.ApplicantName);
            var fatherNameCells = rows[5].Elements<TableCell>().ToList();
            AddTextToCell(fatherNameCells[1], data.FatherName);
            var cnicCells = rows[6].Elements<TableCell>().ToList();
            AddTextToCell(cnicCells[1], ApplicationFormReportingHelper.FormatCNICWithDashes(data.Cnic));
            var emailCells = rows[7].Elements<TableCell>().ToList();
            AddTextToCell(emailCells[1], data.Email);
            var mobileNoCells = rows[8].Elements<TableCell>().ToList();
            AddTextToCell(mobileNoCells[1], ApplicationFormReportingHelper.FormatPhoneNo(data.MobileNo));
            var everExpelledFromUniCells = rows[9].Elements<TableCell>().ToList();
            AddTextToCell(everExpelledFromUniCells[1], data.EverExpelled.ToUpper());
        }
        private static void ReplaceEmergencyContactNo(Table table, EmergencyContactInformationTableModel data)
        {
            var rows = table.Elements<TableRow>().ToList();
            var contactNameCells = rows[1].Elements<TableCell>().ToList();
            AddTextToCell(contactNameCells[1], data.ContactName);
            var relationWithContactCells = rows[2].Elements<TableCell>().ToList();
            AddTextToCell(relationWithContactCells[1], data.ContactRelation);
            var contactPhoneCells = rows[3].Elements<TableCell>().ToList();
            AddTextToCell(contactPhoneCells[1], ApplicationFormReportingHelper.FormatPhoneNo(data.ContactPhone));
            var contactAddressCells = rows[4].Elements<TableCell>().ToList();
            AddTextToCell(contactAddressCells[1], data.ContactAddress);
        }
        private static void ReplaceAppliedProgramsTable(Table table, List<AppliedProgramTableModel> data)
        {
            if (data.Count < 5)
            {
                var remainingRowsCount = 5 - data.Count;
                for (int i = 0; i < remainingRowsCount; i++)
                {
                    data.Add(new AppliedProgramTableModel
                    {
                        DepartmentName = "",
                        Shift = ""
                    });
                }
            }
            var rows = table.Elements<TableRow>();
            int index = 0;
            foreach (var row in rows)
            {
                var cells = row.Elements<TableCell>().ToList();
                AddTextToCell(cells[0], data[index].DepartmentName);
                AddTextToCell(cells[1], data[index].Shift);
                index++;
            }
        }
        private static void AddTextToCell(TableCell cell, string data)
        {
            var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
            // Collect existing run styles
            var existingRunStyles = paragraph!.Descendants<Run>()
                .Select(run => run.RunProperties)
                .FirstOrDefault();
            // Remove all runs
            paragraph.RemoveAllChildren<Run>();
            // Add new runs with retained styles
            // Create a new run and apply the style
            var newRun = new Run();
            // add styles
            if (existingRunStyles != null)
                newRun.RunProperties = (RunProperties)existingRunStyles!.CloneNode(true); // Copy styles
            // Add new text
            newRun.AppendChild(new Text(data));
            // Append the run to the paragraph
            paragraph.AppendChild(newRun);
        }
        public static void ReplacePictureBox(string tagName, string imagePath, WordprocessingDocument doc)
        {
            var drawing = doc.MainDocumentPart!.Document.Body!
                .Descendants<Drawing>()
                .FirstOrDefault(d => d.Descendants<DocProperties>().FirstOrDefault()?.Name == tagName)!;

            DocProperties dpr = drawing.Descendants<DocProperties>().FirstOrDefault()!;
            if (dpr != null)
            {
                var b = drawing.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();


                // Get the old image part
                var oldImagePart = doc.MainDocumentPart.GetPartById(b!.Embed!);
                // Remove the old image part
                doc.MainDocumentPart.DeletePart(oldImagePart);

                // Add a new image part
                var newImagePart = doc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg);

                // Add image data to the new image part
                using (var stream = new FileStream(string.Concat("wwwroot/",imagePath.Split("7081")[1].Remove(0, 1)), FileMode.Open))
                {
                    newImagePart.FeedData(stream);
                }

                // // Update the blip reference to point to the new image
                b.Embed = doc.MainDocumentPart.GetIdOfPart(newImagePart);

                Console.WriteLine("Image replaced successfully.");



            }
        }

        private UGApplicationFormReportModel PerformModelMapping(ApplicationForm data)
        {
            List<AcademicRecordsTableModel> academicRecordsTableModels = data.Applicant!
                        .Degrees!
                        .Select(degree => new AcademicRecordsTableModel
                        {
                            DegreeGroupName = degree.DegreeGroup?.DegreeName ?? "",
                            RollNo = degree.RollNo,
                            BoardOrUniversity = degree.BoardOrUniversityName,
                            ObtainedMarks = degree.ObtainedMarks,
                            TotalMarks = degree.TotalMarks,
                            PassingYear = degree.PassingYear,
                        }).ToList();

            List<AppliedProgramTableModel> appliedProgramTableModels = data.ProgramsApplied!
                .Select(appliedProgram => new AppliedProgramTableModel
                {
                    DepartmentName = appliedProgram.Department!.Name,
                    Shift = appliedProgram.TimeShift!.Name,
                }).ToList();
            PersonalInformationTableModel personalInformation = new PersonalInformationTableModel
            {
                AcademicSessionYearAndProgram = ApplicationFormReportingHelper.CreateAcademicSessionYearAndProgram(data.Session?.Name ?? "", data.Session?.AcademicYear?.Name ?? "", data?.Program?.Name ?? ""),
                FormNo = data!.Id,
                VoucherId = data.FeeChallan!.Id,
                FatherName = data.Applicant.FatherName!,
                ApplicantName = data.Applicant.FullName,
                Cnic = data.Applicant.Cnic,
                Email = data.Applicant.ApplicationUser!.Email!,
                EverExpelled = data.Applicant.ExpelledFromUni!,
                MobileNo = data.Applicant.MobileNo!,
            };

            EmergencyContactInformationTableModel contactInformationTableModel = new EmergencyContactInformationTableModel
            {
                ContactAddress = data.Applicant.EmergencyContact!.PermanentAddress,
                ContactPhone = data.Applicant.EmergencyContact!.ContactNo,
                ContactRelation = data.Applicant.EmergencyContact!.Relation,
                ContactName = data.Applicant.EmergencyContact!.Name,
            };

            TextFieldsModel fields = new TextFieldsModel
            {
                FormId = data.Id,
                ApplicantMobileNo = data.Applicant.MobileNo!,
                AcademicSession = personalInformation.AcademicSessionYearAndProgram,
                ApplicantName = data.Applicant.FullName,
                FatherName = data.Applicant.FatherName!,
                GuardianAddress = data.Applicant.Guardian!.PermanentAddress,
                GuardianName = data.Applicant.Guardian.Name,
                GuardianPhone = data.Applicant.Guardian.ContactNo,
                GuardianRelation = data.Applicant.Guardian.Relation,
                City = data.Applicant.City!,
                Cnic = data.Applicant.Cnic!,
                PostalCode = data.Applicant.PostalCode ?? 0,
                PostalAddress = data.Applicant.PermanentAddress!,
                Dob = data.Applicant.Dob.GetValueOrDefault(),
                Domicile = data.Applicant.Domicile!,
                Female = ApplicationFormReportingHelper.GetTickOrCrossForFemale(data.Applicant.Gender!),
                Male = ApplicationFormReportingHelper.GetTickOrCrossForMale(data.Applicant.Gender!),
                Province = data.Applicant.Province!,
                Date = ApplicationFormReportingHelper.FormatSubmittedDate(data.SubmissionDate.GetValueOrDefault()),
            };
            ProfilePictureModel profilePictureModel = new ProfilePictureModel
            {
                ProfilePicture = data.Applicant.ApplicationUser.ProfilePictureUrl!,
            };
            return new UGApplicationFormReportModel
            {
                AcademicRecordsTableModels = academicRecordsTableModels,
                AppliedProgramTableModel = appliedProgramTableModels,
                EmergencyContactInformation = contactInformationTableModel,
                PersonalInformationTableModel = personalInformation,
                TextFields = fields,
                ProfilePictureModel = profilePictureModel,
            };
        }
        private static List<KeyValuePair<string, string>> MapTextFieldsDataToKeyValue(TextFieldsModel model)
        {
            return
            [
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.ApplicantName, model.ApplicantName.ToUpper()),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.AcademicSession, model.AcademicSession),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.FatherName, model.FatherName.ToUpper()),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Cnic, ApplicationFormReportingHelper.FormatCNIC(model.Cnic)),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Dob, ApplicationFormReportingHelper.FormatDOB(model.Dob)),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Female, model.Female),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Male, model.Male),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Domicile, model.Domicile),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Province, model.Province),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.City, model.City),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.PostalAddress, model.PostalAddress),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.ApplicantMobileNo,ApplicationFormReportingHelper.FormatPhoneNo(model.ApplicantMobileNo)),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.Date,model.Date),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.GuardianName,model.GuardianName.ToUpper()),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.GuardianRelation,model.GuardianRelation.ToUpper()),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.GuardianAddress,model.GuardianAddress),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.GuardianPhone,ApplicationFormReportingHelper.FormatPhoneNo(model.GuardianPhone)),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.FormId,ApplicationFormReportingHelper.NumberToString(model.FormId)),
               new KeyValuePair<string, string>(ApplicationFormReportingHelper.PostalCode,ApplicationFormReportingHelper.NumberToString(model.PostalCode)),
            ];
        }
    }
}