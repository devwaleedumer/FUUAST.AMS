import { z } from "zod";

export const degreeValidator = z.object({
    degrees: z.array(
        z.object({
            degreeGroupId: z.coerce.number({
                required_error: "Please select group",
                invalid_type_error: "select valid group"
            }),
            subject: z
                .string()
                .min(1, { message: "Please enter subject" }),
            boardOrUniversityName: z
                .string({
                    required_error: "Board or University is required",
                    invalid_type_error: "enter valid name"
                }).min(1, { message: "Please enter value" }),
            passingYear: z.coerce
                .number({
                    invalid_type_error: "only digits are required",
                })
                .max(2024).min(1980),
            rollNo: z
                .string({
                    required_error: "Roll no. is required"
                }).min(1, { message: "Please enter Roll No." }),
            totalMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .positive("please enter a valid digit"),
            obtainedMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .positive("please enter a valid digit")


        }).refine(fields => fields.obtainedMarks <= fields.totalMarks, {
            message: "Obtained must be smaller then total marks",
            path: ["obtainedMarks"]
        }),
    )
})

export const editDegreeValidator = z.object({
    degrees: z.array(
        z.object({
            degreeGroupId: z.coerce.number({
                required_error: "Please select group",
                invalid_type_error: "select valid group"
            }),
            applicantId: z.number(),
            id: z.number(),
            subject: z
                .string()
                .min(1, { message: "Please enter subject" }),
            boardOrUniversityName: z
                .string({
                    required_error: "Board or University is required",
                    invalid_type_error: "enter valid name"
                }).min(1, "Please enter value").trim(),
            passingYear: z.coerce
                .number({
                    invalid_type_error: "only digits are required",
                })
                .max(2024).min(1980),
            rollNo: z
                .string({
                    required_error: "Roll no. is required"
                }).min(1)
            ,
            totalMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .positive("please enter a valid digit"),
            obtainedMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .positive("please enter a valid digit")


        }).refine(fields => fields.obtainedMarks <= fields.totalMarks, {
            message: "Obtained must be smaller then total marks",
            path: ["obtainedMarks"]
        }),
    )
})

export type DegreeValues = z.infer<typeof degreeValidator>