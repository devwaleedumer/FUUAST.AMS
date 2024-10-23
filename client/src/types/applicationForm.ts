import { editAdmissionSelectionValues } from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";
import { Shift } from "./shift";
import { Department } from "./department";

export type ApplicationFormCreateResponse = {
    id: number
}

export type ApplicationFormCreateRequest = {
    programId: number;
}

export type SubmitApplicationFormResponse = editAdmissionSelectionValues & {
    shifts: Shift[][],
    departments: Department[][],
}

export type ApplicantDashboardResponse = {
    lastModified?: string,
    completedSteps: number,
    formStatuses: FormStatus[]
}

export type FormStatus = {
    name: string,
    status: string,
}