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
        fatherName: z.string({
            invalid_type_error: "Father name is required"
        })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Full Name must contain only alphabets."
            }),
        permanentAddress: z.string({
            invalid_type_error: "Permanent address is required"
        }).trim().min(1, "Address  is required 😊."),
        cnic: z.string().regex(/^\d{13}$/, "only 13 digits are allowed")
        ,

        guardian: z.object({
            name: z.string({
                invalid_type_error: "invalid input"
            }).min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string({
                required_error: "relation is required", invalid_type_error: "required"
            }).min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string({ required_error: "Permanent address is required", invalid_type_error: "required" }).min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string({ required_error: "Contact is required", invalid_type_error: "required" }).regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        })
        ,
        emergencyContact: z.object({
            name: z.string().min(1, "Name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Contact Name must contain only alphabets."
                }),
            relation: z.string({ required_error: "Relation is required", invalid_type_error: "Relation is required" }).min(1, "Contact relation  is required 😊")
                .trim(),
            permanentAddress: z.string({ required_error: "Address is required", invalid_type_error: "required" }).min(1, "Contact address  is required 😊").trim(),
            contactNo: z.string({ required_error: "Contact is required", invalid_type_error: "required" }).regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        }),
        religion: z.string({ required_error: "Gender is required" }).min(1, { message: "Please select a religion" }),
        gender: z.string({ required_error: "Gender is required" }).min(1, { message: "Please select a gender" }),
        bloodGroup: z.string({ required_error: "Blood group is required" }).min(1, { message: "Please select a blood group" }),
        province: z.string({ required_error: "Province is required", invalid_type_error: "Province is required" }).min(1, { message: "Province is required" })
            .trim()
        ,
        city: z.string({ required_error: "City is required", invalid_type_error: "City is required" }).min(1, { message: "City is required" })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "City must contain only alphabets."
            }),
        country: z.string({ required_error: "Country is required", invalid_type_error: "Country is required" }).min(1, { message: "Country is required" })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "City must contain only alphabets."
            }),
        domicile: z.string({ required_error: "required", invalid_type_error: "required" }).min(1, { message: "Domicile is required" })
            .trim()
        ,
        postalCode: z.coerce.number({ invalid_type_error: "Only digits are allowed", required_error: "required" })
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
            required_error: "required"
        }).regex(/^\d{11}$/, "11 digits are allowed")
        ,
        fatherName: z.string({
            required_error: "required",
            invalid_type_error: "required"
        })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Full Name must contain only alphabets."
            }),
        permanentAddress: z.string({
            invalid_type_error: "required",
            required_error: "required"
        }).trim().min(1, "Address  is required 😊."),
        cnic: z.string().regex(/^\d{13}$/, "only 13 digits are allowed")
        ,

        guardian: z.object({
            name: z.string({ required_error: "required", invalid_type_error: "required" }).min(1, "guardian name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Guardian Name must contain only alphabets."
                }),
            relation: z.string({ required_error: "required", invalid_type_error: "required" }).min(1, "Guardian relation  is required 😊")
                .trim(),
            permanentAddress: z.string({ required_error: "required", invalid_type_error: "required" }).min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string({ required_error: "required", invalid_type_error: "required" }).regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        })
        ,
        emergencyContact: z.object({
            name: z.string({ required_error: "Name is required", invalid_type_error: "Name is required" }).min(1, "Contact name is required")
                .trim()
                .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                    message: "Contact Name must contain only alphabets."
                }),
            relation: z.string({ required_error: "Relation is required", invalid_type_error: "required" }).min(1, "Contact relation  is required 😊")
                .trim(),
            permanentAddress: z.string({ required_error: "Address is required", invalid_type_error: "Address is required" }).min(1, "Guardian address  is required 😊").trim(),
            contactNo: z.string({ required_error: "Contact no is  required", invalid_type_error: "Contact no is  required" }).regex(/^\d{11}$/, "11 digits are allowed"),
            applicantId: z.number().optional(),
            id: z.number().optional()
        }),
        religion: z.string({ required_error: "Religion is required" }).min(1, { message: "Please select a religion" }),
        gender: z.string({ required_error: "Gender is required" }).min(1, { message: "Please select a gender" }),
        bloodGroup: z.string({ required_error: "blood group is required" }).min(1, { message: "Please select a blood group" }),
        province: z.string({ required_error: "Province is required", invalid_type_error: "Please select a Province" }).min(1, { message: "Province is required" })
            .trim()
        ,
        city: z.string({ required_error: "City is required", invalid_type_error: "City is required" }).min(1, { message: "City is required" })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "City must contain only alphabets."
            }),
        country: z.string({ required_error: "required", invalid_type_error: "required" }).min(1, { message: "Country is required" })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "City must contain only alphabets."
            }),
        domicile: z.string({ required_error: "Domicile is required", invalid_type_error: "Domicile is required" }).min(1, { message: "Domicile is required" })
            .trim()
            .regex(/^[A-Za-z]+( [A-Za-z]+)*$/, {
                message: "Domicile must contain only alphabets."
            }),
        postalCode: z.coerce.number({ invalid_type_error: "Only digits are allowed", required_error: "required" })
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
    cnic: z.string().regex(/^\d{13}$/, "only 13 digits are allowed")
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