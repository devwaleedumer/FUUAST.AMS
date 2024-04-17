import * as z from "zod";

export const profileSchema = z.object({
    fullName: z
        .string()
        .min(3, { message: "Full Name must be at least 3 characters" }),
    email: z
        .string()
        .email({ message: "email must be at least 3 characters" }),

    dob: z
        .string()
        .refine((value) => /^\d{4}-\d{2}-\d{2}$/.test(value), {
            message: "Start date should be in the format YYYY-MM-DD",
        }),
    contactNo: z.coerce.number(),
    religion: z.string().min(1, { message: "Please select a religion" }),
    gender: z.string().min(1, { message: "Please select a gender" }),
    disabled: z.boolean(),
    disabilityDetails: z.string().min(5, "details must be at least 5 characters"),
    bloodGroup: z.string().min(1, { message: "Please select a blood group" }),
    domicileProvince: z.string().min(1, { message: "Please select a Province" }),
    domicileDistrict: z.string().min(1, { message: "Please select a District" }),
    // jobs array is for the dynamic fields
    jobs: z.array(
        z.object({
            jobcountry: z.string().min(1, { message: "Please select a category" }),
            jobcity: z.string().min(1, { message: "Please select a category" }),
            jobtitle: z
                .string()
                .min(3, { message: "Product Name must be at least 3 characters" }),
            employer: z
                .string()
                .min(3, { message: "Product Name must be at least 3 characters" }),
            startdate: z
                .string()
                .refine((value) => /^\d{4}-\d{2}-\d{2}$/.test(value), {
                    message: "Start date should be in the format YYYY-MM-DD",
                }),
            enddate: z.string().refine((value) => /^\d{4}-\d{2}-\d{2}$/.test(value), {
                message: "End date should be in the format YYYY-MM-DD",
            }),
        }),
    ),
});

export type ProfileFormValues = z.infer<typeof profileSchema>;