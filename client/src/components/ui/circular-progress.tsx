import { cn } from '@/lib/utils'
import React, { FC } from 'react'

type Props = {
    percentage: number,
}

export const CircularProgress = React.forwardRef<
  HTMLDivElement,
  React.HTMLAttributes<HTMLParagraphElement> & Props 
>(({ className, percentage, ...props }, ref) => {
  return (
    <div className={cn("relative ",className)}>
          <svg className="size-full -rotate-90" viewBox="0 0 36 36" xmlns="http://www.w3.org/2000/svg">
    {/* <!-- Background Circle --> */}
    <circle cx="18" cy="18" r="16" fill="none" className="stroke-current text-gray-200 dark:text-neutral-700" strokeWidth="2"></circle>
    {/* <!-- Progress Circle --> */}
    <circle cx="18" cy="18" r="16" fill="none" className="stroke-current text-green-600 dark:text-green-500" strokeWidth="2" strokeDasharray="100" strokeDashoffset={100-percentage} strokeLinecap="round"></circle>
  </svg>
  <div className="absolute top-1/2 start-1/2 transform -translate-y-1/2 -translate-x-1/2">
    <span className="text-center text-2xl font-bold text-green-600 dark:text-green-500">{percentage}%</span>
  </div>
    </div>
)})
CircularProgress.displayName = "CircularProgress"
export default CircularProgress;