"use client";
import Header from "@/components/Layouts/Dashboard/Header";
import Sidebar from "@/components/Layouts/Dashboard/Sidebar";
import LayoutLoader from "@/components/shared/Loader";
import { Suspense, useEffect, useState } from "react";

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
