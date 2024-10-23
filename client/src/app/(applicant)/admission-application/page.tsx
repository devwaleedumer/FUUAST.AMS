"use client"
import BreadCrumb from "@/components/shared/Breadcrumb";
import { Application } from "@/components/ApplicantForm/StepperForm";
import { ScrollArea } from "@/components/ui/scroll-area";

const breadcrumbItems = [{ title: "Application", link: "/ug-application" }];

export default function Page() {

  return (
      <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 px-6 py-4">
        <BreadCrumb items={breadcrumbItems} />
          <Application  />
      </div>
    </ScrollArea>
  );
}
