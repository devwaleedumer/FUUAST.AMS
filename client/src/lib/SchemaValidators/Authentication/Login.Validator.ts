import { z } from "zod";

export const loginFormSchema = z.object({
    email: z.string().email("Enter  a valid email").min(6, "Email must contain minimum 6 character(s)"),
    password: z.string().min(8, "Your Password must contain minimum 8 character(s)")
})
export type loginFormSchemaType = z.infer<typeof loginFormSchema>