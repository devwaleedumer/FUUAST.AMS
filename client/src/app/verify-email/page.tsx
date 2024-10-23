"use client"
import { toast } from '@/components/ui/use-toast'
import VerifyEmail from '@/components/VerifyEmail/VerifyEmail'
import { CheckCircle2 } from 'lucide-react'
import React from 'react'

const page = () => {
  return (
    // <VerifyEmail/>
    <button onClick={() => {
       toast({
      title: "Application Submitted 🎓!",
      description: "We've received your admission form.",
      action: (
        <div className="flex items-center">
          <CheckCircle2 className="mr-2 h-5 w-5 text-green-500" />
          <span className="font-semibold text-green-500">Success</span>
        </div>
      ),
      className: "bg-white dark:bg-gray-800 border-green-500 border-2",
      duration: 5000,
    })
    }}>
        BB
    </button>
  )
}

export default page