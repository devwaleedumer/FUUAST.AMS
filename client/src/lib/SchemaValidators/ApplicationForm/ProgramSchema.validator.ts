import * as z from "zod";

export const programValidator = z.object({
    programId: z.coerce.string({
        required_error: "Please select a program to proceed",
        invalid_type_error: "invalid input"
    })
});

export type ProgramValues = z.infer<typeof programValidator>;