import { Icons } from "@/components/Icons";

export interface NavItem {
    title: string;
    href?: string;
    disabled?: boolean;
    external?: boolean;
    icon?: keyof typeof Icons;
    label?: string;
    description?: string;
}

// import { Icons } from "@/components/Layouts/DashboardTwo/Icons";

// export interface NavItem {
//     title: string;
//     url: string;
//     disabled?: boolean;
//     external?: boolean;
//     icon?: keyof typeof Icons;
//     label?: string;
//     description?: string;
//     isActive?: boolean;
//     items?: NavItem[];
// }
