import { Icons } from "@/components/Icons";
import { Dispatch, SetStateAction } from "react";

export interface NavItem {
    title: string;
    href?: string;
    disabled?: boolean;
    external?: boolean;
    icon?: keyof typeof Icons;
    label?: string;
    description?: string;
}

export interface ApplicationForm {
    wizardStep1?: WizardStep1,
    wizardStep2?: WizardStep2,
    wizardStep3?: WizardStep3
    completed: boolean
}
// step#1 program information
export interface ProgramInfo {
    programType: number,
    program1: number,
    program2?: number,
    program3?: number,
    program4?: number
}

export interface PersonalInformation {
    fullName: string,
    cnic: string,
    dob: string,
    religion: string,
    gender: string,
    bloodGroup: string,
    isDisabled: boolean,
    disabilityDetails?: boolean,
    whatsappNo: string,
    nextOfKinName?: string,
    nextOfKinRelation?: string,
    domicileDistrict: string,
    domicileProvince: string
    parentInfo: ParentInfo,
    guardian: Guardian,

}

export interface EmergencyContactAndAddress {
    addresses: Address[],
    emergencyContact: EmergencyContact,
}

export interface WizardStep1 {
    personalInformation: PersonalInformation,
    completed: boolean,
}

export interface WizardStep2 {
    emergencyContactAndAddress: EmergencyContactAndAddress,
    completed: boolean
}

export interface WizardStep3 {
    degrees: Degree[],
    completed: boolean

}


export interface ParentInfo {
    fatherName: string,
    fatherOccupation: string,
    motherName: string,
    FatherCnic: string,
    IsFatherDeceased: boolean
}

export interface Guardian {
    name: string,
    occupation: string,
    relation: string,
    totalIncome: number,
    totalExpense: number,
    phoneNumber: string,
}

export interface EmergencyContact {
    name: string,
    occupation: string,
    relation: string,
    phoneNumber: string,
}

export interface Address {
    streetAddress: string,
    province: string,
    district: string,
    portalCode: number,
    AddressType: number
}

export interface Degree {
    degreeLevel: number,
    degreeName: string,
    MajorSubject: string,
    instituteName: string,
    BoardOrUniversityName: string,
    fromYear: string,
    toYear: string,
    rollNo: string,
    gradingType: number,
    examType: number,
    totalMarks: number,
    obtainedMarks: number,
    percentage: number,
}

// initialize application form
export const initializeApplicationForm: ApplicationForm = {
    wizardStep1: {
        personalInformation: {
            fullName: "",
            religion: "",
            whatsappNo: "",
            cnic: "",
            gender: "",
            dob: "",
            bloodGroup: "",
            isDisabled: false,
            domicileDistrict: "",
            domicileProvince: "",
            nextOfKinName: "",
            nextOfKinRelation: "",
            parentInfo: {
                fatherName: "",
                motherName: "",
                fatherOccupation: "",
                FatherCnic: "",
                IsFatherDeceased: false,
            },
            guardian: {
                name: "",
                relation: "",
                phoneNumber: "",
                occupation: "",
                totalExpense: 0,
                totalIncome: 0,
            },
        },
        completed: false,
    },
    wizardStep2: {
        emergencyContactAndAddress: {
            emergencyContact: {
                name: "",
                phoneNumber: "",
                occupation: "",
                relation: "",
            },
            addresses: [
                {
                    streetAddress: "",
                    province: "",
                    district: "",
                    portalCode: 0,
                    AddressType: 1,
                },
                {
                    streetAddress: "",
                    province: "",
                    district: "",
                    portalCode: 0,
                    AddressType: 2,
                },
            ],
        },
        completed: false,
    },
    wizardStep3: {
        degrees: [],
        completed: false
    },
    completed: false,
}

export interface FormContextType {
    setStep1Answered: Dispatch<SetStateAction<boolean>>,
    setStep2Answered: Dispatch<SetStateAction<boolean>>,
    setStep3Answered: Dispatch<SetStateAction<boolean>>,
    setFinished: Dispatch<SetStateAction<boolean>>,
    setStepData: Dispatch<SetStateAction<ApplicationForm>>,
    finished: boolean
    step1Answered: boolean,
    step2Answered: boolean,
    step3Answered: boolean,
    stepData: ApplicationForm

}