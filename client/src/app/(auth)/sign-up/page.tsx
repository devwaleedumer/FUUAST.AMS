"use client";
import isAuth from "@/components/ApplicantProtected";
import Signup from "@/components/Authentication/Signup/Signup";
import React from "react";

type Props = {};

const page = () => {
  return (
    <>
      <Signup />
    </>
  );
};

export default page