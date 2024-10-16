import { admissionSelectionValidator } from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";
import { degreeValidator } from "@/lib/SchemaValidators/ApplicationForm/DegreeSchema.validator";
import { personalInfo, personalInfoEditSchema } from "@/lib/SchemaValidators/ApplicationForm/PersonalInfoSchema.validator";
import { programValidator } from "@/lib/SchemaValidators/ApplicationForm/ProgramSchema.validator";
import { z } from "zod";

// type AdmissionSelection = z.infer<typeof admissionSelectionValidator>
// type Degrees = z.infer<typeof degreeValidator>
export type PersonalInformation = Omit<z.infer<typeof personalInfoEditSchema>, 'imageRequest' | 'profileImage'>
export type ApplicantDegrees = {
    degrees: {
        applicantId: number,
        id: number,
        degreeGroupId: number;
        subject: string;
        boardOrUniversity: string;
        passingYear: number;
        rollNo: string;
        totalMarks: number;
        obtainedMarks: number;
    }[];
}

export type Degree = {
    applicantId: number,
    id: number,
    degreeGroupId: number;
    subject: string;
    boardOrUniversity: string;
    passingYear: number;
    rollNo: string;
    totalMarks: number;
    obtainedMarks: number;

}