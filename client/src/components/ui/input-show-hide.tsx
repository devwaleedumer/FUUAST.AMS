import * as React from "react";

import { cn } from "@/lib/utils";
import { AiOutlineEye, AiOutlineEyeInvisible } from "react-icons/ai";

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  isPassword?: boolean;
  showPassword?: boolean;
  setShowPassword?: (showPassword: boolean) => void;
}

const InputShowHide = React.forwardRef<HTMLInputElement, InputProps>(
  (
    { className, type, isPassword, showPassword, setShowPassword, ...props },
    ref
  ) => {
    return (
      <div className="relative">
        <input
          type={type}
          className={cn(
            "flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50",
            className
          )}
          ref={ref}
          {...props}
        />
        {isPassword &&
          setShowPassword &&
          (!showPassword ? (
            <AiOutlineEyeInvisible
              className="absolute bottom-3 right-1 z-1 cursor-pointer"
              size={20}
              onClick={() => setShowPassword(true)}
            />
          ) : (
            <AiOutlineEye
              className="absolute bottom-3 right-1 z-1 cursor-pointer"
              size={20}
              onClick={() => setShowPassword(false)}
            />
          ))}
      </div>
    );
  }
);
InputShowHide.displayName = "InputShowHide";

export { InputShowHide };
