import { z } from "zod";
const getCharacterValidationError = (str: string) => {
    return `Your password must have at least 1 ${str} character`;
};
export const signUpFormSchema = z.object({
    fullName: z.string().min(2, "Full name must contain at least 2 character(s)").regex(/^[A-Za-z\s]+$/, {
        message: 'Full Name  must contain only alphabetic character.',
    }),
    email: z.string().min(5, "Email must contain at least 5 character(s)").email(),
    cnic: z.string().regex(/^\d{13}$/, "only 13 digits are allowed"),
    password: z.string()
        .regex(/[0-9]/, getCharacterValidationError("digit"))
        .regex(/[a-z]/, getCharacterValidationError("lowercase"))
        .regex(/[A-Z]/, getCharacterValidationError("uppercase")),
    confirmPassword: z.string().min(8, "Confirm Password must contain at least 8 character(s)")
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
});

export type SignupValues = z.infer<typeof signUpFormSchema>