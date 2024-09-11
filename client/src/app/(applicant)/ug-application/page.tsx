"use client"
import BreadCrumb from "@/components/shared/Breadcrumb";
import { Application } from "@/components/ApplicantForm/StepperForm";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Suspense, useEffect, useState } from "react";
import LayoutLoader from "@/components/shared/Loader";

const breadcrumbItems = [{ title: "UG Form", link: "/ug-application" }];
export default function Page() {

  return (
      <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 p-4 md:p-8 pt-6">
        <BreadCrumb items={breadcrumbItems} />
          <Application categories={[]} initialData={null} />
      </div>
    </ScrollArea>
  );
}
