"use client";
import Header from "@/components/Layout/Header";
import Sidebar from "@/components/Layout/Sidebar";

export default function ApplicantDashboardLayout({
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
