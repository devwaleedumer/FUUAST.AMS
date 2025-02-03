// import { NavItem } from "@/types";

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
        title: "Challan",
        href: "/application-challan",
        icon: "kanban",
        label: "Challan",
    },
    {
        title: "Profile",
        href: "/change-password",
        icon: "profile",
        label: "profile",
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
    { id: "Male", name: "Male" },
    { id: "Female", name: "Female" },
    { id: "Others", name: "Others" },
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

// export const navItems: NavItem[] = [
//     {
//         title: 'Dashboard',
//         url: '/dashboard/overview',
//         icon: 'dashboard',
//         isActive: false,
//         items: [] // Empty array as there are no child items for Dashboard
//     },
//     {
//         title: 'Employee',
//         url: '/dashboard/employee',
//         icon: 'user',
//         isActive: false,
//         items: [] // No child items
//     },
//     {
//         title: 'Product',
//         url: '/dashboard/product',
//         icon: 'userPen',
//         isActive: false,
//         items: [] // No child items
//     },
//     {
//         title: 'Account',
//         url: '#', // Placeholder as there is no direct link for the parent
//         icon: 'billing',
//         isActive: true,

//         items: [
//             {
//                 title: 'Profile',
//                 url: '/dashboard/profile',
//                 icon: 'userPen'
//             },
//             {
//                 title: 'Login',
//                 url: '/',
//                 icon: 'login'
//             }
//         ]
//     },
//     {
//         title: 'Kanban',
//         url: '/dashboard/kanban',
//         icon: 'kanban',
//         isActive: false,
//         items: [] // No child items
//     }
// ];

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
        // "Other",
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


export const pakistanCitiesByProvince = [
    {
        province: "Punjab",
        cities: [
            "Lahore",
            "Faisalabad",
            "Rawalpindi",
            "Multan",
            "Gujranwala",
            "Sargodha",
            "Bahawalpur",
            "Sialkot",
            "Sheikhupura",
            "Jhang",
            "Rahim Yar Khan",
            "Kasur",
            "Sahiwal",
            "Okara",
            "Murree",
            "Attock",
            "Mianwali",
            "Toba Tek Singh",
            "Hafizabad",
            "Narowal",
        ],
    },
    {
        province: "Sindh",
        cities: [
            "Karachi",
            "Hyderabad",
            "Sukkur",
            "Larkana",
            "Nawabshah (Shaheed Benazirabad)",
            "Mirpur Khas",
            "Jacobabad",
            "Shikarpur",
            "Khairpur",
            "Badin",
            "Thatta",
            "Sanghar",
            "Umerkot",
            "Tando Adam",
            "Tando Allahyar",
            "Matiari",
            "Jamshoro",
            "Dadu",
            "Naushahro Feroze",
        ],
    },
    {
        province: "Khyber Pakhtunkhwa (KPK)",
        cities: [
            "Peshawar",
            "Abbottabad",
            "Mardan",
            "Swat (Mingora)",
            "Nowshera",
            "Charsadda",
            "Swabi",
            "Dera Ismail Khan",
            "Mansehra",
            "Kohat",
            "Bannu",
            "Haripur",
            "Battagram",
            "Shangla",
            "Buner",
            "Lower Dir",
            "Upper Dir",
            "Malakand",
            "Lakki Marwat",
        ],
    },
    {
        province: "Balochistan",
        cities: [
            "Quetta",
            "Gwadar",
            "Turbat",
            "Khuzdar",
            "Chaman",
            "Hub",
            "Sibi",
            "Zhob",
            "Loralai",
            "Dera Murad Jamali",
            "Usta Muhammad",
            "Pasni",
            "Panjgur",
            "Nushki",
            "Qila Saifullah",
            "Ziarat",
            "Kalat",
            "Mastung",
            "Kharan",
        ],
    },
    {
        province: "Gilgit-Baltistan",
        cities: [
            "Gilgit",
            "Skardu",
            "Hunza",
            "Nagar",
            "Ghizer",
            "Shigar",
            "Astore",
            "Diamer",
            "Ghanche",
            "Kharmang",
        ],
    },
    {
        province: "Azad Jammu and Kashmir (AJK)",
        cities: [
            "Muzaffarabad",
            "Mirpur",
            "Kotli",
            "Rawalakot",
            "Bagh",
            "Bhimber",
            "Neelum Valley",
            "Hattian Bala",
            "Sudhnuti",
            "Palandri",
        ],
    },
];

export const relations = [
    "Father",
    "Mother",
    "Uncle",
    "Aunt",
    "Grandfather",
    "Grandmother",
    "Brother",
    "Sister",
    "Guardian",
    "Stepfather",
    "Stepmother",
    "Other",
];

export const countrie = [
    "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
    "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain",
    "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia",
    "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina Faso",
    "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic",
    "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Croatia", "Cuba",
    "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador",
    "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji",
    "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala",
    "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia",
    "Iran", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya",
    "Kiribati", "Korea, North", "Korea, South", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho",
    "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia", "Maldives",
    "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco",
    "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands",
    "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Panama",
    "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia",
    "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino",
    "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore",
    "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Sudan", "Spain", "Sri Lanka",
    "Sudan", "Suriname", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor-Leste",
    "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates",
    "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen",
    "Zambia", "Zimbabwe"
];


const allCitiesInPakistan = [
    "Karachi", "Lahore", "Islamabad", "Rawalpindi", "Faisalabad", "Multan", "Peshawar", "Quetta",
    "Sialkot", "Gujranwala", "Bahawalpur", "Sukkur", "Muzaffargarh", "Mardan", "Larkana", "Hyderabad",
    "Gujrat", "Chiniot", "Kasur", "Jhang", "Dera Ghazi Khan", "Sargodha", "Mirpur Khas", "Khairpur",
    "Zhob", "Abbottabad", "Swat", "Mansehra", "Bannu", "Kotli", "Nawabshah", "Tando Adam", "Nowshera",
    "Attock", "Chakwal", "Dera Ismail Khan", "Mingora", "Shikarpur", "Khushab", "Mardan", "Dera Ghazi Khan",
    "Jhelum", "Rawalakot", "Bhakkar", "Wah Cantt", "Fateh Jang", "Samundri", "Tando Allahyar", "Sahiwal",
    "Chak Jhumra", "Kohat", "Skardu", "Mithi", "Mirpur", "Bhalwal", "Shahdadkot", "Lodhran", "Burewala",
    "Bannu", "Ghotki", "Risalpur", "Jaranwala", "Chaman", "Gojra", "Karak", "Haripur", "Mianwali",
    "Kohlu", "Muzaffarabad", "Bajaur", "Hangu", "Kohistan", "Sadiqabad", "Malkani", "Jamalpur", "Burewala",
    "Tando Muhammad Khan", "Sialkot", "Jampur", "Kohat", "Kashmore", "Layyah", "Bannu", "Dera Ismail Khan",
    "Chiniot", "Sahiwal", "Khanewal", "Fatehpur", "Faisalabad", "Nowshera", "Sargodha", "Turbat", "Loralai",
    "Mastung", "Panjgur", "Ziarat", "Jaffarabad", "Kalat", "Kech", "Nushki", "Kharan", "Chaghi", "Kohistan",
    "Dera Bugti", "Loralai", "Killa Saifullah", "Shahdadpur", "Mithi", "Parachinar", "Azad Kashmir", "Srinagar",
    "Jhelum", "Hangu", "Balochistan", "Pishin", "Sibi", "Lasbela", "Kohat", "Nowshera", "Sukkur", "Badin",
    "Tando Allahyar", "Mian Channu", "Fateh Jang", "Wazirabad", "Jhelum", "Daharki", "Pind Dadan Khan", "Shikarpur",
    "Chakwal", "Swabi", "Khushab", "Hasilpur", "Toba Tek Singh", "Shorkot", "Layyah", "Chiniot", "Shujaabad",
    "Awaran", "Khairpur", "Sargodha", "Shahkot", "Kotli", "Nowshera", "Chiniot", "Samundri", "Mardan",
    "Peshawar", "Sialkot", "Bahawalnagar", "Jaranwala", "Mianwali", "Ghotki", "Mirpur", "Jaffarabad"
];
export const allBoardsAndUniversities = [
    // Boards of Education
    "BISE Lahore", "BISE Rawalpindi", "BISE Faisalabad", "BISE Multan", "BISE Sargodha",
    "BISE Sahiwal", "BISE Gujranwala", "BISE Bahawalpur", "BISE Dera Ghazi Khan", "BISE Sialkot",
    "BISE Karachi", "BISE Hyderabad", "BISE Sukkur", "BISE Larkana", "BISE Mirpurkhas",
    "BISE Peshawar", "BISE Mardan", "BISE Swat", "BISE Abbottabad", "BISE Kohat",
    "BISE Quetta", "BISE Balochistan", "BISE Azad Jammu & Kashmir", "FBISE",

    // Public Universities
    "Quaid-i-Azam University, Islamabad", "University of the Punjab, Lahore",
    "National University of Sciences and Technology (NUST), Islamabad", "Aga Khan University, Karachi",
    "COMSATS University Islamabad", "University of Karachi", "University of Engineering and Technology (UET) Lahore",
    "University of Peshawar", "University of Sargodha", "University of Faisalabad",
    "Balochistan University of Information Technology, Engineering and Management Sciences (BUITEMS)",
    "Khyber Medical University", "University of Azad Jammu & Kashmir", "The Islamia University of Bahawalpur",
    "University of Science and Technology (FAST)", "University of Engineering and Technology (UET) Taxila",
    "University of Sindh", "University of Balochistan", "Pakistan Institute of Engineering and Applied Sciences (PIEAS)",

    // Private Universities
    "Lahore University of Management Sciences (LUMS), Lahore", "Institute of Business Administration (IBA), Karachi",
    "Habib University, Karachi", "Shaheed Zulfikar Ali Bhutto Institute of Science & Technology (SZABIST), Karachi",
    "Preston University, Islamabad", "Abasyn University, Peshawar", "Sultan Mohammad Shah Aga Khan School, Karachi",
    "University of Management and Technology (UMT), Lahore", "Roots International University & School, Islamabad",
    "Iqra University, Karachi", "Bahria University, Islamabad", "Mohammad Ali Jinnah University (MAJU), Karachi",
    "Lahore School of Economics, Lahore", "Institute of Engineering & Technology, Islamabad",
    "University of Central Punjab, Lahore"
];
