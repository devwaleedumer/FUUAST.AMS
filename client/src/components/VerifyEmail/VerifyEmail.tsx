'use client'
import { useState, useEffect } from 'react'
import { Loader2, CheckCircle, XCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { useVerifyEmailQuery } from '@/redux/features/auth/userApi'
import {  useSearchParams } from 'next/navigation'
import Link from 'next/link'

type VerificationState = 'verifying' | 'success' | 'error'

export default function VerifyEmail() {
  const [state, setState] = useState<VerificationState>('verifying')
  const [message, setMessage] = useState<string>()
  const params = useSearchParams()
  const userId = params.get('userId')
  const code = params.get('code')
  const cnic = params.get('cnic')
  const fullName = params.get('fullName')
  const {isSuccess,data,error,isLoading} = useVerifyEmailQuery({userId: String(userId),code: String(code),cnic: String(cnic),fullName: String(fullName)});
  useEffect(() => {

  },[state])
  useEffect(() => {
     if (isSuccess) {
       setMessage(data);
      setState(isSuccess ? 'success' : 'error');
     }
     if (error) {
       if ("data" in error ) {
        const errorData = error.data as any ;
            setMessage(errorData.detail)
        }
     }
  },[isSuccess,data,error])
 

  const content =  {
       'verifying':
         {
          icon: <Loader2 className="w-8 h-8 animate-spin text-primary" />,
          title: 'Verifying your email',
          description: 'Please wait while we verify your email address...'
        },
       'success':
         {
          icon: <CheckCircle className="w-8 h-8 text-green-500" />,
          title: 'Email Verified',
          description: message ? message :'Your email has been successfully verified. You can now close this window.'
        },
       'error':
         {
          icon: <XCircle className="w-8 h-8 text-red-500" />,
          title: 'Verification Failed',
          description: message ? message : 'We couldn\'t verify your email. The link may have expired or is invalid.'
        }
  }


  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <Card className="w-full max-w-md transition-all duration-300 ease-in-out">
        <CardHeader>
          <div className="flex justify-center mb-4">
            {isLoading ?  content["verifying"].icon : data || isSuccess ? content["success"].icon :content["error"].icon}
          </div>
          <CardTitle className="text-center">{isLoading ?  content["verifying"].title : data || isSuccess ? content["success"].title :content["error"].title}</CardTitle>
          <CardDescription className="text-center">{isLoading ?  content["verifying"].description : isSuccess || data ? content["success"].description :content["error"].description}</CardDescription>
        </CardHeader>
        <CardContent>
          {error && (
            <Button 
              className="w-full mt-4"
              onClick={() => window.location.reload()}
            >
              Try Again
            </Button>
          )}
          {data && (
           <Link href={"/login"}>
            <Button 
              className="w-full mt-4"
            >
              Login
            </Button>
           </Link>
          )}
        </CardContent>
      </Card>
    </div>
  )
}