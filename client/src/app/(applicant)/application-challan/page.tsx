"use client"
import Challan from '@/components/challan/Challan'
import BreadCrumb from '@/components/shared/Breadcrumb';
import { ScrollArea } from '@/components/ui/scroll-area';
import React from 'react'

const breadcrumbItems = [{ title: "Application", link: "/ug-application" }];

export default function Page() {

  return (
      <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 px-6 py-4">
        <BreadCrumb items={breadcrumbItems} />
          <Challan  />
      </div>
    </ScrollArea>
  );
}