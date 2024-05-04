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
        invalid_type_error: "contact can be only number 😻",
    })
        .min(11, "Contact no. must  contains at least 11 digits")
        .max(11, "Contact no.  contains maximum 11 digits")

    ,
    cnic: z.coerce.number()
        .min(13, "CNIC must  contains at least 13 digits")
        .max(13, "CNIC   contains maximum 13 digits"),
    isFatherDeceased: z.coerce.boolean().default(false).optional(),
    fatherName: z.string().max(30, "name cannot exceed 30 chars 🥴")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    motherName: z.string().max(30, "name cannot exceed 30 chars ")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    fatherOccupation: z.string().max(20, "max 20 chars are allowed"),
    fatherContact: z.coerce.number({
        invalid_type_error: "only digits are allowed 🤭"
    })
        .min(11, "Contact no. must  contains at least 11 digits")
        .max(11, "Contact no.  contains maximum 11 digits")
        .int("only digits are allowed 🤭"),
    nextOfKin: z.string().min(1, "Next of kin name is required")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    nextOfKinRelation: z.string().min(1, "Next of kin relation is required")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    guardianName: z.string().min(1, "guardian name is required")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed 😊"),
    isFatherGuardian: z.coerce.boolean().default(false).optional(),
    guardianRelation: z.string().min(1, "guardian relation  is required 😊")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    guardianOccupation: z.string().min(1, "Occupation is required 😊"),
    guardianContact: z.coerce.number()
        .min(11, "Contact no. must  contains at least 11 digits 🤨")
        .max(11, "Contact no.  contains maximum 11 digits 🤨")
        .int("only digits are allowed 🤭"),

    guardianTotalIncome: z.coerce.number({
        invalid_type_error: "Only numbers are allowed"
    })
        .positive("Please specify valid range 🙂")
        .int("decimals are not allowed 🤭"),

    guardianTotalExpenses: z.coerce.number({
        invalid_type_error: "Only numbers are allowed"
    })
        .positive("Please specify valid range 🙂")
        .int("decimals are not allowed 🤭")
    , emergencyCName: z.string().min(1, " name is required")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed 😊"),
    emergencyCRelation: z.string().min(1, " relation  is required 😊")
        .regex(/^[a-zA-Z]+$/, "only alphabets are allowed"),
    emergencyCContact: z.coerce.number()
        .min(11, "Contact no. must  contains at least 11 digits 🤨")
        .max(11, "Contact no.  contains maximum 11 digits 🤨")
        .int("only digits are allowed 🤭"),
    addresses: z.array(z.object({
        streetAddress: z.string().min(5, "Street address must contain  at least 5 chars"),
        addressProvince: z.string().min(1, { message: "Please select a Province" }),
        addressDistrict: z.string().min(1, { message: "Please select a District" }),
        addressId: z.number(),
        addressPostalCode: z.number().int("only digits are allowed 🤭").positive("enter valid postal code").max(5, "Postal code contains max 5 digits"),
    })),
    religion: z.string().min(1, { message: "Please select a religion" }),
    gender: z.string().min(1, { message: "Please select a gender" }),
    bloodGroup: z.string().min(1, { message: "Please select a blood group" }),
    domicileProvince: z.string().min(1, { message: "Please select a Province" }),
    domicileDistrict: z.string().min(1, { message: "Please select a District" }),

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
}).superRefine((ref, ctx) => {
    if (ref.guardianTotalIncome < ref.guardianTotalExpenses) {
        ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: "Expense should not exceed total income 😪",
            path: ["guardianTotalExpenses"]
        })
    }
    return z.NEVER;
});



export type ProfileFormValues = z.infer<typeof profileSchema>;