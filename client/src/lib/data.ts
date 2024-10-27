import { NavItem } from "@/types";

export const navItems: NavItem[] = [
    {
        title: "Dashboard",
        href: "/dashboard",
        icon: "dashboard",
        label: "Dashboard",
    },
    {
        title: "Application",
        href: "/admission-application",
        icon: "wizard",
        label: "Application Wizard",
    },
    {
        title: "Profile",
        href: "/profile",
        icon: "profile",
        label: "profile",
    },
    {
        title: "Challan",
        href: "/dashboard/Challan",
        icon: "kanban",
        label: "Challan",
    },

];


export const countries = [
    { id: "Punjab", name: "Punjab" },
    { id: "KPK", name: "KPK" },
    { id: "Balouchistan", name: "Balouchistan" },
    { id: "Punjab", name: "Punjab" },
    { id: "GB", name: "GB" },
    { id: "Kashmir", name: "Kashmir" },
    { id: "Islamabad", name: "Islamabad" },
];
export const cities = [
    { id: "1", name: "Islamabad" },
    { id: "2", name: "Peshawar" },
    { id: "3", name: "Rawalpindi" },
    { id: "4", name: "Lahore" },
    { id: "5", name: "Karachi" },
    { id: "6", name: "Hyderabad" },
    { id: "7", name: "Sialkot" },
    { id: "8", name: "Faisalabad" },
];

export const genders = [
    { id: "1", name: "Male" },
    { id: "2", name: "Female" },
    { id: "3", name: "Others" },
];

export const bloodGroups = [
    { id: "A+", name: "A+" },
    { id: "A-", name: "A-" },
    { id: "B+", name: "B+" },
    { id: "B-", name: "B-" },
    { id: "O+", name: "O+" },
    { id: "O-", name: "O-" },
    { id: "AB+", name: "AB+" },
    { id: "AB-", name: "AB-" },
];

export const religions = [
    { id: "Islam", name: "Islam" },
    { id: "Christianity", name: "Christianity" },
    { id: "Judaism", name: "Judaism" },
    { id: "Hinduism", name: "Hinduism" },
    { id: "Atheist", name: "Atheist" },
    { id: "Other", name: "Other" },
];


export const photographRequirements = [
    "Must be taken recently",
    "Should be in white or blue background",
    "Should be in formal dressing (Photograph in T-shirts containing logos and brand names are prohibited)",
    "Should be clear, sharp, and of good quality",
    "Full face must be visible (70-80% of the photograph)",
    "Eye contact with the camera",
    "Should have natural skin tone",
    "With natural brightness and contrast"
];

export const importantNotesAdmissionSelection = [
    "Order of choice of the disciplines once submitted Should not be changed under any circumstances.",
    "You can add more programs by clicking on Add more button.",
    "You can remove program by clicking on bin or remove icon.",
    "Applying for 1 program will cost a challan of 2000 (default)",
    "Note: Applying for more then one program will be charged 2000 each.",
    "i.e Applying for three programs will cost. 3*2000 = Rs. 6000",
    "For any Further query contact on 03359055577"
]

export const academicDegreeNamesRecord = {
    "1": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
    ],
    "2": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
    ],
    "3": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        // "Other",
    ],
    "4": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        // "Other",
    ],
    "5": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        "MS/M.Phill/MS/18 years education",
        // "Other",
    ],
    "6": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        // "Other",
    ],
    "7": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        // "Other",
    ],

} as const
export type AcademicDegreeNamesRecordParam = "1" | "2" | "3" | "4" | "5" | "6" | "7"
export type AcademicDegreeNamesRecordType = typeof academicDegreeNamesRecord[AcademicDegreeNamesRecordParam]