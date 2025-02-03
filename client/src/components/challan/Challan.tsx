import React, { useEffect, useState } from 'react'
import { Button } from '../ui/button'
import {  AlertCircle, Download, Eye, PenLine, ReceiptText } from 'lucide-react'
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from '../ui/card'
import { Badge } from '../ui/badge'
import { Table, TableBody, TableCell, TableRow } from '../ui/table'
import { Heading } from '../ui/heading'
import { Alert, AlertDescription, AlertTitle } from '../ui/alert'
import { ProfileFileUpload } from '../ApplicantForm/ProfileFileUpload'
import { useForm } from 'react-hook-form'
import { imageUploaderSchema, ImageUploaderValues } from '@/lib/SchemaValidators/Image.Validator'
import { zodResolver } from '@hookform/resolvers/zod'
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '../ui/form'
import { useGetApplicationDetailQuery } from '@/redux/features/applicationForm/applicationFormApi'
import { useAppSelector } from '@/hooks/reduxHooks'
import PageLoader from '../shared/Loader'
import { Console } from 'console'
import { useUploadChallanImageMutation } from '@/redux/features/feeChallan/feeChallanApi'
import { toast } from '../ui/use-toast'
import Link from 'next/link'
// import { useLazyGetFeeChallanQuery } from '@/redux/features/feeChallan/feeChallanApi'

type Props = {}
const title = "Challan and Application Form";
const description =
  "Download and Submit Challan after confirming challan. You will be allowed to download Application form";
const Challan = (props: Props) => {
    const form = useForm<ImageUploaderValues>({
      resolver: zodResolver(imageUploaderSchema),
      mode: "onBlur",
    });
    const [isChallanLoading, setIsChallanLoading] = useState(false)
    const applicantId = useAppSelector(x => x.auth.user?.applicantId)
    const {data,isLoading: applicationDetailLoading, isSuccess : applicationDetailSuccess, isError: applicationDetailError } = useGetApplicationDetailQuery(applicantId as number,{}) 
    const [admissionFormLoading, setAdmissionFormLoading] = useState(false)
    // const [generatePdf, { isLoading: isChallanLoading, isError }] = useLazyGetFeeChallanQuery();
  const handleDownload = async () => {
    setIsChallanLoading(true);
    fetch(`https://localhost:7081/api/FeeChallans/get-challan/${applicantId}`, {
      method: "GET",
      credentials: "include",
    })
.then((res) => {
    return res.blob();
})
.then((blob) => {
    const href = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = href;
    link.setAttribute('download', 'fee_challan.pdf'); //or any other extension
    document.body.appendChild(link);
    link.click();
    setIsChallanLoading(false);
    document.body.removeChild(link);
})
.catch((err) => {
    return Promise.reject({ Error: 'Something Went Wrong', err });
})

  }

   const handleDownloadForm = async () => {
    setAdmissionFormLoading(true);
    fetch(`https://localhost:7081/api/ApplicationForms/report/${data?.formNo}`, {
      method: "GET",
      credentials: "include",
    })
.then((res) => {
    return res.blob();
})
.then((blob) => {
    const href = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = href;
    link.setAttribute('download',  `form-${data?.fullName}-${data?.formNo}-${data?.program}.pdf`); //or any other extension
    document.body.appendChild(link);
    link.click();
    setAdmissionFormLoading(false);
    document.body.removeChild(link);
})
.catch((err) => {
    return Promise.reject({ Error: 'Something Went Wrong', err });
})

  }
  const [uploadChallanImage,{isLoading: uploadChallanImageLoading, isSuccess: uploadImageIsSuccess}] = useUploadChallanImageMutation();
  const handleFormSubmit = async (dataImage: ImageUploaderValues) => {
   await uploadChallanImage({feeChallanId: parseInt(data?.feeChallanNo as string),data:dataImage})
    console.log(dataImage.imageRequest)
  } 


  useEffect(() => {
    if (uploadImageIsSuccess) {
      toast({
        title: "Challan uploaded successfully",
        description: "Challan uploaded successfully!",
        variant: "default"
      })
    }
  },[uploadImageIsSuccess])
  
    const {
    setValue,
    formState: { errors },
  } = form;    
  return (
    applicationDetailLoading ?
    <PageLoader/>
    :
     <>
    {applicationDetailSuccess && data ? 
      <>
        <Heading title={title} description={description} />
     <div className="min-h-screen mt-4 ">
      <Card className="w-full mx-auto bg-white">
        <CardHeader className="border-b">
          <CardTitle className="text-2xl font-bold  text-gray-800">Application Details</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableBody>
              {Object.entries(data).map(([key, value]) => (
                <TableRow key={key} className="hover:bg-gray-50">
                  <TableCell className="font-medium text-gray-600 w-1/3 py-4">
                    {key.charAt(0).toUpperCase() + key.slice(1).replace(/([A-Z])/g, ' $1').trim()}:
                  </TableCell>
                  <TableCell className="text-right py-4">
                    {key === 'challanStatus' ? (
                      <Badge variant={value === "Paid" ? "default" : "destructive"}>
                        {value}
                      </Badge>
                    ) : (
                      <span className="text-gray-900">{value}</span>
                    )}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
           <div className=" flex flex-col sm:flex-row justify-end space-y-2 sm:space-y-0 sm:space-x-2 w-full bg-gray-50 p-6  border-t">
          <Button className="w-full sm:w-auto" disabled={isChallanLoading} onClick={handleDownload}>
            <ReceiptText className="mr-2 h-4 w-4" /> Generate Challan
          </Button>
        {
          data.challanStatus == "Paid" &&  
            <Button disabled={admissionFormLoading}  onClick={() => handleDownloadForm()} variant="outline" className="w-full sm:w-auto">
            <Download className="mr-2 h-4 w-4" /> Download  Form
          </Button>
        }
        </div>
        </CardContent>
      </Card>
     {
     data.challanStatus !== "Paid" &&  <>
         <div className='border-t my-5 relative'>
            <span className=' absolute -top-[12px] text-gray-600 px-2  left-1/2 bg-white'>OR</span>
      </div>

      <Form {...form} >
        <form onSubmit={form.handleSubmit(handleFormSubmit, (error) => console.log(error))}>
        <Card className='pt-5'>
          <CardContent>
                  <FormField
                        control={form.control}
                        name="profileImage"
                        render={({ field }) => (
                          <FormItem className="mb-2">
                            <FormLabel className="relative">
                              Submit Paid  Challan here:{" "}
                            </FormLabel>
                            <FormControl>
                              <ProfileFileUpload
                                setValue={setValue}
                                onChange={field.onChange}
                                value={field.value}
                                isValid={!!errors.profileImage}
                                className='w-[500px] min-h-48'
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      </CardContent>
                      <CardFooter>
                      <div className="  flex flex-col sm:flex-row justify-end space-y-2 sm:space-y-0 sm:space-x-2 w-full rounded">
                      <Button disabled={uploadChallanImageLoading} className="w-full sm:w-auto" type="submit">
                      <ReceiptText className="mr-2 h-4 w-4" /> Upload Challan
                      </Button>
                      </div>
                      </CardFooter>
                      </Card>    
        </form>
      </Form>
      </>
     }
    </div> 
      </>
      :
       <CardContent className="p-6">
              <Alert variant="destructive" className="mb-6">
                <AlertCircle className="h-4 w-4" />
                <AlertTitle>Application Not Filled</AlertTitle>
                <AlertDescription>
                  You haven t filled out your application form yet. Please complete the form to proceed.
                </AlertDescription>
              </Alert>
              <Button className="w-full">
                <PenLine className="mr-2 h-4 w-4" /> Fill Application Form
              </Button>
            </CardContent>
    }
     </>
  )
}
// IF application not found
  // <CardContent className="p-6">
  //             <Alert variant="warning" className="mb-6">
  //               <AlertCircle className="h-4 w-4" />
  //               <AlertTitle>Application Not Filled</AlertTitle>
  //               <AlertDescription>
  //                 You haven't filled out your application form yet. Please complete the form to proceed.
  //               </AlertDescription>
  //             </Alert>
  //             <Button className="w-full">
  //               <PenLine className="mr-2 h-4 w-4" /> Fill Application Form
  //             </Button>
  //           </CardContent>
//           )}

export default Challan