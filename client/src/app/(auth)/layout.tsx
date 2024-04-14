import React, { FC, ReactNode } from "react";

type AuthLayoutProps = {
  children: ReactNode;
};

const Authlayout: FC<AuthLayoutProps> = ({ children }) => {
  return <>{children}</>;
};

export default Authlayout;
