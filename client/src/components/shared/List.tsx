import { cn } from "@/lib/utils";
import { GreenTick } from "@/utils/icons/CustomIcons";
import React from "react";

type ListProps = {
    list: string[];
    title: string
  };

const List = React.forwardRef<
  HTMLDivElement,
  React.HTMLAttributes<HTMLParagraphElement> & ListProps 
>(({ className, title,list, ...props }, ref) => {
  return (
    <div className={cn(className, "py-5 mx-0")} {...props} ref={ref}>
      <h2 className="mb-2  tracking-tighter  font-semibold  text-gray-900 dark:text-white">
        {title}
      </h2>
      <ul className="max-w-md space-y-1 text-gray-700 list-inside dark:text-gray-400">
        {list &&
          list.map((req, index) => (
            <li key={req + index} className="flex items-center text-sm ">
              <GreenTick className="w-3.5 h-3.5 me-2"/> {req}
            </li>
          ))}
      </ul>
    </div>
  );
});

List.displayName = "List";
export default List;
