import React from "react";

interface HeadingProps {
  title: string;
  description: string;
  className? : string
}

export const Heading: React.FC<HeadingProps> = ({ title, description,className  }) => {
  return (
    <div className={className}>
      <h2 className="md:text-3xl text-2xl font-bold tracking-tight">{title}</h2>
      <p className="text-sm text-muted-foreground">{description}</p>
    </div>
  );
};
