import * as z from "zod";
import { personalInfo } from "./PersonalInfoSchema.validator";
import { degreeValidator } from "./DegreeSchema.validator";
export const applicantFullInfoCreateSchema = z.object({
    applicant: personalInfo,
    degrees: degreeValidator,
})