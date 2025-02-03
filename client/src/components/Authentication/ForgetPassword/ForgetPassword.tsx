import { Card, CardContent, CardHeader } from '@/components/ui/card'
import { FormLabel } from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import React, { FC } from 'react'

type Props = {}

const ForgetPassword : FC<Props> = ({}) => {
  return (
    <div className='min-h-screen bg-gray-100 py-8 px-4 sm:px-6 lg:px-8 flex items-center justify-center'>
        <div className="w-full max-w-2xl">
            <Card>
                <CardHeader>
                   Enter Your Mail to recover  your account
                </CardHeader>
                <CardContent>
                <FormLabel>Enter  Mail</FormLabel>
                <Input type='email' />
                </CardContent>
            </Card>
        </div>
    </div>
  )
}

export default ForgetPassword