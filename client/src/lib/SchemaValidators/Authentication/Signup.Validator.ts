import { z } from "zod";
const getCharacterValidationError = (str: string) => {
    return `Your password must have at least 1 ${str} character`;
};
export const signUpFormSchema = z.object({
    fullName: z.string().min(5, "Full name must contain at least 5 character(s)")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed in name"),

    email: z.string().min(5, "Email must contain at least 5 character(s)").email(),
    password: z.string()
        .regex(/[0-9]/, getCharacterValidationError("digit"))
        .regex(/[a-z]/, getCharacterValidationError("lowercase"))
        .regex(/[A-Z]/, getCharacterValidationError("uppercase")),
    confirmPassword: z.string().min(8, "Confirm Password must contain at least 8 character(s)")
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
});

export type signUpFormSchemaType = z.infer<typeof signUpFormSchema>