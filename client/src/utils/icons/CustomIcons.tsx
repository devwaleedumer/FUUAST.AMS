import { cn } from "@/lib/utils";
import React from "react";
import { FC } from "react";


const GreenTick =  React.forwardRef<
  HTMLOrSVGElement,
  React.HTMLAttributes<HTMLOrSVGElement>
>(({ className, ...props }, ref) => {
  return (
     <svg
        {...props}
        className={cn(" text-green-500 dark:text-green-400 flex-shrink-0",className)}
        aria-hidden="true"
        xmlns="http://www.w3.org/2000/svg"
        fill="currentColor"
        viewBox="0 0 20 20"
    >
        <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z" />
 </svg>
  )
})

GreenTick.displayName = "GreenTick"

export {GreenTick}