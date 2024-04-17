import BreadCrumb from "@/components/shared/Breadcrumb";
import { CreateProfileOne } from "@/components/ApplicantForm/StepperForm";
import { ScrollArea } from "@/components/ui/scroll-area";

const breadcrumbItems = [{ title: "Profile", link: "/dashboard/profile" }];
export default function page() {
  return (
    <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 p-4 md:p-8 pt-6">
        <BreadCrumb items={breadcrumbItems} />
        <CreateProfileOne categories={[]} initialData={null} />
      </div>
    </ScrollArea>
  );
}
