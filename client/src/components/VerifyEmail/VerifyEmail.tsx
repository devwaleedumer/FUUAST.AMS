'use client'
import { useState, useEffect } from 'react'
import { Loader2, CheckCircle, XCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { useLazyVerifyEmailQuery } from '@/redux/features/auth/userApi'
import { useRouter } from 'next/router'

type VerificationState = 'verifying' | 'success' | 'error'

export default function VerifyEmail() {
  const [state, setState] = useState<VerificationState>('verifying')
  const [message, setMessage] = useState<string>()
  const router = useRouter()
  const {userId,code} = router.query;
  const [verifyEmail,{isSuccess}] = useLazyVerifyEmailQuery();
  useEffect(() => {
    if (userId && code) {
     verifyEmail({userId: userId as string, code: code as string }).unwrap().then( (data) => {
      setMessage(data);
      setState(isSuccess ? 'success' : 'error');
    }).catch((error) => {
        if ("data" in error) {
            setMessage(error.data.detail)
        }
    });
    }
  }, [userId,code])

  const getContent = () => {
    switch (state) {
      case 'verifying':
        return {
          icon: <Loader2 className="w-8 h-8 animate-spin text-primary" />,
          title: 'Verifying your email',
          description: 'Please wait while we verify your email address...'
        }
      case 'success':
        return {
          icon: <CheckCircle className="w-8 h-8 text-green-500" />,
          title: 'Email Verified',
          description: message ? message :'Your email has been successfully verified. You can now close this window.'
        }
      case 'error':
        return {
          icon: <XCircle className="w-8 h-8 text-red-500" />,
          title: 'Verification Failed',
          description: message ? message : 'We couldn\'t verify your email. The link may have expired or is invalid.'
        }
    }
  }

  const content = getContent()

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <Card className="w-full max-w-md transition-all duration-300 ease-in-out">
        <CardHeader>
          <div className="flex justify-center mb-4">
            {content.icon}
          </div>
          <CardTitle className="text-center">{content.title}</CardTitle>
          <CardDescription className="text-center">{content.description}</CardDescription>
        </CardHeader>
        <CardContent>
          {state === 'error' && (
            <Button 
              className="w-full mt-4"
              onClick={() => window.location.reload()}
            >
              Try Again
            </Button>
          )}
          {state === 'success' && (
            <Button 
              className="w-full mt-4"
              onClick={() => router.replace("/login")}
            >
              Login
            </Button>
          )}
        </CardContent>
      </Card>
    </div>
  )
}