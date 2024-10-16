"use client";
import isAuth from "@/components/ApplicantProtected";
import Header from "@/components/Layouts/Dashboard/Header";
import Sidebar from "@/components/Layouts/Dashboard/Sidebar";

 function ApplicantDashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
      <>
      <Header />
      <div className="flex h-screen overflow-hidden">
        <Sidebar />
          <main className="w-full pt-16">{children}</main>
      </div>
     </>
  );
}
export default isAuth(ApplicantDashboardLayout,"optional")