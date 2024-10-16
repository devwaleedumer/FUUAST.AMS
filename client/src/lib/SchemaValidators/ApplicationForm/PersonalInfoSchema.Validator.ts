import { ACCEPTED_IMAGE_TYPES, MAX_FILE_SIZE } from "@/lib/configurations/UploaderConf";
import * as z from "zod";
export const personalInfoEditSchema = z.discriminatedUnion("isImageChanged", [
    z.object({
        id: z.number(),
        profileImage: z
            .instanceof(File, {
                message: "Image is required"
            })
            .refine((file) => file?.size <= MAX_FILE_SIZE, `Max image size is 4MB.`)
            .refine(
                (file) => ACCEPTED_IMAGE_TYPES.includes(file?.type),
                "Only .jpg, .jpeg, .png and .webp formats are supported."
            ),
        isImageChanged: z.literal(true),
        imageRequest: z.object({
            name: z.string(),
            extension: z.string(),
            data: z.string()
        }),
        dob: z.date
            ({
                required_error: "date of birth is required",
                invalid_type_error: "date of birth is invalid",
            }),
        mobileNo: z.string({
            invalid_type_error: "contact no  can be only number 😻.",
        }).regex(/^\d{11}$/, "11 digits are allowed")
        ,
        fatherName: z.string()
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Full Name must contain only alphabets."
            }),
        permanentAddress: z.string().trim().min(1, "Address  is required 😊."),
        cnic: z.string().regex(/^\d{13}/, "only 13 digits are allowed")
        ,

        guardian: z.object({
            name: z.string().min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string().min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        })
        ,
        emergencyContact: z.object({
            name: z.string().min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string().min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        }),
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
        postalCode: z.coerce.number({ invalid_type_error: "Only digits are allowed" })
    }),
    z.object({
        id: z.number(),
        profilePictureUrl: z.string(),
        isImageChanged: z.literal(false),
        dob: z.date
            ({
                required_error: "date of birth is required",
                invalid_type_error: "date should be in the format YYYY-MM-DD",
            }),
        mobileNo: z.string({
            invalid_type_error: "contact no  can be only number 😻.",
        }).regex(/^\d{11}$/, "11 digits are allowed")
        ,
        fatherName: z.string()
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Full Name must contain only alphabets."
            }),
        permanentAddress: z.string().trim().min(1, "Address  is required 😊."),
        cnic: z.string().regex(/^\d{13}/, "only 13 digits are allowed")
        ,

        guardian: z.object({
            name: z.string().min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string().min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        })
        ,
        emergencyContact: z.object({
            name: z.string().min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string().min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        }),
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
        postalCode: z.coerce.number({ invalid_type_error: "Only digits are allowed" })
    })
])
export const personalInfo = z.object({
    profileImage: z
        .instanceof(File, {
            message: "Image is required"
        })
        .refine((file) => file?.size <= MAX_FILE_SIZE, `Max image size is 4MB.`)
        .refine(
            (file) => ACCEPTED_IMAGE_TYPES.includes(file?.type),
            "Only .jpg, .jpeg, .png and .webp formats are supported."
        ),
    imageRequest: z.object({
        name: z.string(),
        extension: z.string(),
        data: z.string()
    }),
    dob: z.date
        ({
            required_error: "date of birth is required",
            invalid_type_error: "date should be in the format YYYY-MM-DD",
        }),
    mobileNo: z.string({
        invalid_type_error: "contact no  can be only number 😻.",
    }).regex(/^\d{11}$/, "11 digits are allowed")
    ,
    fatherName: z.string()
        .trim()
        .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
            message: "Full Name must contain only alphabets."
        }),
    permanentAddress: z.string().trim().min(1, "Address  is required 😊."),
    cnic: z.string().regex(/^\d{13}/, "only 13 digits are allowed")
    ,

    guardian: z.object({
        name: z.string().min(1, "guardian name is required")
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Guardian Name must contain only alphabets."
            }),
        relation: z.string().min(1, "Guardian relation  is required 😊")
            .trim(),
        permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
        contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
    })
    ,
    emergencyContact: z.object({
        name: z.string().min(1, "guardian name is required")
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Guardian Name must contain only alphabets."
            }),
        relation: z.string().min(1, "Guardian relation  is required 😊")
            .trim(),
        permanentAddress: z.string().min(1, "Guardian address  is required 😊").trim(),
        contactNo: z.string().regex(/^\d{11}$/, "11 digits are allowed"),
    }),
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
    postalCode: z.coerce.number({ invalid_type_error: "Only digits are allowed" })
})



export type PersonalInfoValues = z.infer<typeof personalInfo>;
export type PersonalEditInfoValues = z.infer<typeof personalInfoEditSchema>;

// PersonalInfo default values
export const personalInfoDefaults: Partial<PersonalEditInfoValues> = {
    mobileNo: '',
    fatherName: '',
    cnic: '',
    country: '',
    province: '',
    domicile: '',
    city: '',
    permanentAddress: '',
    bloodGroup: '',
    guardian: {
        name: '',
        relation: '',
        contactNo: '',
        permanentAddress: '',
    },
    emergencyContact: {
        name: '',
        relation: '',
        contactNo: '',
        permanentAddress: '',
    }

}