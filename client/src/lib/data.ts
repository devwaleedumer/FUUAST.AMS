import { NavItem } from "@/types";

export const navItems: NavItem[] = [
    {
        title: "Application",
        href: "/admission-application",
        icon: "dashboard",
        label: "Dashboard",
    },
    // {
    //     title: "User",
    //     href: "/dashboard/user",
    //     icon: "user",
    //     label: "user",
    // },
    // {
    //     title: "Employee",
    //     href: "/dashboard/employee",
    //     icon: "employee",
    //     label: "employee",
    // },
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
    {
        title: "Logout",
        href: "/",
        icon: "login",
        label: "Logout",
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


// export const degreeLevel = [
//     { id: "SSC", name: "SSC" },
//     { id: "HSSC", name: "HSSC" },
//     { id: "Diploma 13 years (DAE)", name: "Diploma 13 years (DAE)" },
//     { id: "4", name: "ADP or equivalent 14 Years" },
//     { id: "5", name: "16 Years" },
//     { id: "6", name: "MS 18 Years" },
// ];

// export const examType = [
//     { id: "1", name: "Annual" },
//     { id: "2", name: "Semester" },
// ]

// export const gradingType = [
//     { id: "1", name: "Percentage" },
//     { id: "2", name: "CGPA" },
// ]

// export const programsInfo = [
//     { id: "1", name: "BS" },
//     { id: "2", name: "APD" },
//     { id: "3", name: "BPD" },
//     { id: "4", name: "MBA" },
//     { id: "5", name: "M.phil" },
//     { id: "6", name: "MS" },
//     { id: "7", name: "Phd" },
// ]

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
        "Other",
    ],
    "4": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        "Other",
    ],
    "5": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        "MS/M.Phill/MS/18 years education",
        "Other",
    ],
    "6": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        "Other",
    ],
    "7": [
        "Matriculation/OLevels/SCC/Equivalent",
        "Intermedidate/ALevels/HSSC/DAE/Equivalent",
        "BS/M.Sc/16 years education",
        "Other",
    ],

} as const
export type AcademicDegreeNamesRecordParam = "1" | "2" | "3" | "4" | "5" | "6" | "7"
export type AcademicDegreeNamesRecordType = typeof academicDegreeNamesRecord[AcademicDegreeNamesRecordParam]