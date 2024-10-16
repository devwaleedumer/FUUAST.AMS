import isAuth from '@/components/ApplicantProtected';
import Profile from '@/components/Profile/Profile';
import BreadCrumb from '@/components/shared/Breadcrumb'
import Heading from '@/components/shared/Heading';
import { ScrollArea } from '@/components/ui/scroll-area'
import React from 'react'

const title = "Profile settings"
const description  = "You can change your profile"
type Props = {}
const breadcrumbItems = [{ title: "Profile", link: "/profile" }];

 function  Page () {
  return (
   <>
     <Heading title={title} description={description} keywords="Profile,Settings, Profile Settings"/>
    <ScrollArea className="h-full">
      <div className="flex-1 space-y-4 p-4 md:p-8 pt-6">
        <BreadCrumb items={breadcrumbItems} />
        <Profile/>
      </div>
    </ScrollArea>
   </>
  )
}

export default isAuth(Page,"optional")
