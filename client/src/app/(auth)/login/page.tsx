"use client";
import isAuth from "@/components/ApplicantProtected";
import Login from "@/components/Authentication/Login/Login";
import React from "react";

type Props = {};

const page = (props: Props) => {
  return (
    <>
      <Login />
    </>
  );
};

export default isAuth(page,"auth");
