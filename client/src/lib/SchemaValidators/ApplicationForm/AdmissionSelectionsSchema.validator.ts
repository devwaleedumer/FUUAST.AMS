import * as z from "zod";

export const admissionSelectionValidator = z.object({
    expelledFromUni: z.string({
        invalid_type_error: "Please select one option"
    }),
    heardAboutUniFrom: z.string({
        invalid_type_error: "Please select one option"
    }),
    infoConsent: z.literal(true, {
        required_error: "you should agree before submitting application",
        invalid_type_error: "you should agree before submitting application"
    }),
    programsApplied: z.array(z.object({
        facultyId: z.coerce.number({
            required_error: "Faculty is required",
            invalid_type_error: "Faculty is required",
        }),
        departmentId: z.coerce.number({
            required_error: "Department is required",
            invalid_type_error: "Department is required",
        }),
        timeShiftId: z.coerce.number({
            required_error: "TimeShift is required",
            invalid_type_error: "TimeShift is required",
        }),
    }))
})

export type admissionSelectionValues = z.infer<typeof admissionSelectionValidator>