"use client"
import isAuth from '@/components/ApplicantProtected';
import Dashboard from '@/components/Dashboard/Dashboard';
import BreadCrumb from '@/components/shared/Breadcrumb'
import PageLoader from '@/components/shared/Loader';
import { ScrollArea } from '@/components/ui/scroll-area'
import { useAppSelector } from '@/hooks/reduxHooks';
import { RootState } from '@/redux/store';
import React from 'react'

const breadcrumbItems = [{ title: "Dashboard", link: "/dashboard" }];
const Page = () => {
    const { user } = useAppSelector((state: RootState) => state.auth);

  return (
    <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 px-6 py-4 ">
        <BreadCrumb items={breadcrumbItems} />
        {user ?  <Dashboard user={user}/> : <PageLoader/>}
      </div>
    </ScrollArea>
  )
}

export default Page