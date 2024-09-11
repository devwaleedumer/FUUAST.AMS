import { z } from "zod";

export const degreeValidator = z.array(
    z.object({
        degreeGroupId: z.coerce.number({
            required_error: "Please select group"
        }),
        subject: z
            .string()
            .min(1, { message: "Please enter subject" }),
        boardOrUniversity: z
            .string()
            .optional(),
        passingYear: z
            .coerce
            .number({
                invalid_type_error: "only digits are required",
            })
            .positive("please enter a valid digit"),
        rollNo: z
            .coerce
            .string({
            })
        ,
        totalMarks: z.coerce.number({
            invalid_type_error: "only digits are required",
        })
            .max(4, "Total marks cannot exceed 4 digits")
            .positive("please enter a valid digit"),
        obtainedMarks: z.coerce.number({
            invalid_type_error: "only digits are required",
        })
            .positive("please enter a valid digit")
            .max(4, "Total marks cannot exceed 4 digits")


    }).refine(fields => fields.obtainedMarks <= fields.totalMarks, {
        message: "Obtained must be smaller then total marks",
        path: ["obtainedMarks"]
    }),
)

type DegreeValues = z.infer<typeof degreeValidator>