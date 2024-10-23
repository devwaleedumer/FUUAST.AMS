"use client";
import React, { FC } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import {
  BarChart,
  BookOpen,
  ExternalLink,
  FileCheck,
  FileText,
  GraduationCap,
  Hand,
  PlusCircle,
  User,
} from "lucide-react";
import { Progress } from "../ui/progress";
import { Badge } from "../ui/badge";
import Link from "next/link";
import { Button } from "../ui/button";
import { useAppSelector } from "@/hooks/reduxHooks";
import { RootState } from "@/redux/store";
import { useGetApplicantDashboardDataQuery } from "@/redux/features/applicationForm/applicationFormApi";
import PageLoader from "../shared/Loader";
import { format, formatDistanceToNow } from "date-fns";
import { IUser } from "@/types/auth";

type DashboardProps = {
  user: Partial<IUser>
};
const Dashboard : FC<DashboardProps> = ({user}) => {
  const { data, isLoading,error } = useGetApplicantDashboardDataQuery(
    user?.id as number,{
      refetchOnFocus:true
    }
  );
  return isLoading && data == undefined && !error ?  (
    <PageLoader />
  ) : (
    // Dashboard grid
    <div className="">
      <div className="flex flex-col md:flex-row md:justify-between  mb-4">
        <h1 className="text-2xl font-bold flex">
          Hello,{" "}
          <p className="font-medium ml-1 tracking-tight">{user?.fullName}</p>
        </h1>
        <div className="place-self-end">
          <div className="text-sm font-semibold">
            Last Login:{" "}
            <span className="text-muted-foreground font-normal">
              {format(Date.now(), "do MMM, yyyy hh:ss")}
            </span>
          </div>
        </div>
      </div>
      <div className="grid md:grid-cols-2 gap-6">
        {/* Start Application Card */}
        <Card className="shadow-lg hover:shadow-xl transition-shadow">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-lg">Start Application</CardTitle>
            <PlusCircle className="h-4 w-4 text-primary" />
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <p className="text-sm text-muted-foreground">
                Begin or continue your application process for the upcoming
                academic year.
              </p>
              {data?.completedSteps == 0 ? (
                <Link href={"/admission-application"}>
                  <Button className="w-full">Start Application</Button>
                </Link>
              ) : (
                <>
                  <Link href={"/admission-application"}>
                    <Button className="w-full">Continue Application</Button>
                  </Link>

                  <div className="text-xs text-center text-muted-foreground">
                    Last saved:{" "}
                    {formatDistanceToNow(data?.lastModified ?? Date.now())}
                  </div>
                </>
              )}
            </div>
          </CardContent>
        </Card>
        <Card className=" shadow-lg hover:shadow-xl transition-shadow">
          <CardHeader className="flex flex-row justify-between space-y-0 pb-2">
            <CardTitle className="text-lg">Application Status</CardTitle>
            <BarChart className="size-4 text-primary self-end" />
          </CardHeader>
          <CardContent>
            <div className="text-xl font-bold">
              {(data?.completedSteps as number) * 25}%
            </div>
            <Progress
              value={(data?.completedSteps as number) * 25}
              className="mt-4"
            />
            <p className="text-xs text-muted-foreground mt-2">
              {(data?.completedSteps as number) * 25}% remaining to complete
              your application
            </p>
          </CardContent>
        </Card>

        {/* Forms Status Card */}
        <Card className="shadow-lg hover:shadow-xl transition-shadow">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-lg">Forms Status</CardTitle>
            <FileCheck className="size-4  text-primary" />
          </CardHeader>
          <CardContent>
            <ul className="space-y-2 mt-2">
              {data?.formStatuses.map((item, index) => (
                <li key={index} className="flex justify-between items-center">
                  <span className="text-sm">{item.name}</span>
                  <Badge
                    variant={
                      item.status === "Completed"
                        ? "default"
                        : item.status === "In Progress"
                        ? "outline"
                        : "destructive"
                    }
                  >
                    {item.status}
                  </Badge>
                </li>
              ))}
            </ul>
          </CardContent>
        </Card>
        {/* Quick Links Card */}
        <Card className="shadow-lg hover:shadow-xl transition-shadow">
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-lg">Quick Links</CardTitle>
            <ExternalLink className="h-4 w-4 text-primary" />
          </CardHeader>
          <CardContent>
            <ul className="space-y-2">
              {[
                { text: "Application Guidelines", icon: BookOpen },
                { text: "FAQ", icon: FileText },
                { text: "Contact Admissions Office", icon: User },
              ].map((item, index) => (
                <li key={index}>
                  <Link
                    href="#"
                    className="text-primary hover:text-primary/90 hover:underline flex items-center group"
                  >
                    <item.icon className="h-4 w-4 mr-2 transition-transform group-hover:translate-x-1" />
                    {item.text}
                  </Link>
                </li>
              ))}
            </ul>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default Dashboard;
