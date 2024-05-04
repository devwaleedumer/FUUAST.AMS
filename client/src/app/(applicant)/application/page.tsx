"use client"
import BreadCrumb from "@/components/shared/Breadcrumb";
import { CreateProfileOne } from "@/components/Forms/ApplicantForm/StepperForm";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Suspense } from "react";

const breadcrumbItems = [{ title: "Profile", link: "/dashboard/profile" }];
export default function page() {
  return (
    <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 p-4 md:p-8 pt-6">
        <BreadCrumb items={breadcrumbItems} />
        <Suspense fallback={<div>Loading...</div>} >
          <CreateProfileOne categories={[]} initialData={null} />
        </Suspense>
      </div>
    </ScrollArea>
  );
}
