import { ACCEPTED_IMAGE_TYPES, IMG_MAX_LIMIT, MAX_FILE_SIZE } from "@/lib/configurations/UploaderConf";
import * as z from "zod";


export const profileSchema = z.object({
    profileImage: z
        .instanceof(File)
        .refine((file) => file?.size <= MAX_FILE_SIZE, `Max image size is 4MB.`)
        .refine(
            (file) => ACCEPTED_IMAGE_TYPES.includes(file?.type),
            "Only .jpg, .jpeg, .png and .webp formats are supported."
        ),
    dob: z
        .string()
        .refine((value) => /^\d{4}-\d{2}-\d{2}$/.test(value), {
            message: "Start date should be in the format YYYY-MM-DD",
        }),
    contactNo: z.coerce.number({
        invalid_type_error: "contact no  can be only number 😻.",
    })
        .min(11, "Contact no. must  contains at least 11 digits.")
    // .max(11, "Contact no.  contains maximum 11 digits.")
    ,
    fatherName: z.string().min(1, "Father name is required.")
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Full Name must contain only alphabets."
        }),
    permanentAddress: z.string().trim().min(1, "Address  is required 😊."),
    cnic: z.coerce.number({
        invalid_type_error: "Only digits are allowed"
    }).refine((number) => number.toString().length === 13, "must contains 13 digits")

    ,

    guardianName: z.string().min(1, "guardian name is required")
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Guardian Name must contain only alphabets."
        }),
    guardianRelation: z.string().min(1, "Guardian relation  is required 😊")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    guardianPermanentAddress: z.string().min(1, "Guardian address  is required 😊"),
    guardianContact: z.coerce.number()
        .min(11, "Contact no. must  contains at least 11 digits 🤨")
    // .int("only digits are allowed 🤭")
    ,
    emergencyCName: z.string().min(3, " Name is required")
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Emergency Name must contain only alphabets."
        }),
    emergencyCRelation: z.string().min(1, " relation  is required 😊")
    ,
    emergencyCContact: z.coerce.number({
        required_error: "Only digits required"
    }).min(11, "Contact no. must  contains at least 11 digits 🤨"),
    emergencyCPermanentAddress: z.string().min(1, "Address  is required 😊"),

    religion: z.string().min(1, { message: "Please select a religion" }),
    gender: z.string().min(1, { message: "Please select a gender" }),
    bloodGroup: z.string().min(1, { message: "Please select a blood group" }),
    province: z.string().min(1, { message: "Province is required" })
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Province must contain only alphabets."
        }),
    city: z.string().min(1, { message: "City is required" })
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "City must contain only alphabets."
        }),
    country: z.string().min(1, { message: "Country is required" })
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "City must contain only alphabets."
        }),
    domicile: z.string().min(1, { message: "Domicile is required" })
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Domicile must contain only alphabets."
        }),
    postalCode: z.coerce.number()
        .min(11, "Must  contains at least 2 digits 🤨"),
    // jobs array is for the dynamic fields
    degrees: z.array(
        z.object({
            degreeLevel: z.string().min(1, { message: "Please select a degree level" }),
            degreeName: z.string().min(1, { message: "Please Select a degree" }),
            degreeMajor: z
                .string()
                .min(1, { message: "Please enter major subject" }),
            boardOrUniversity: z
                .string()
                .optional(),
            institution: z
                .string()
                .optional(),
            startingYear: z
                .coerce
                .number({
                    invalid_type_error: "only digits are required",
                })
                .positive("please enter a valid digit"),

            endingYear: z.coerce
                .number({
                    invalid_type_error: "only digits are required",
                })
                .positive("please enter a valid digit"),


            totalMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .max(4, "Total marks cannot exceed 4 digits")
                .min(4, "Total marks should be of 4 digits")
                .positive("please enter a valid digit"),
            obtainedMarks: z.coerce.number({
                invalid_type_error: "only digits are required",
            })
                .positive("please enter a valid digit")
                .max(4, "Total marks cannot exceed 4 digits")
                .min(4, "Total marks should be of 4 digits"),

        }),
    ),
})



export type ProfileFormValues = z.infer<typeof profileSchema>;