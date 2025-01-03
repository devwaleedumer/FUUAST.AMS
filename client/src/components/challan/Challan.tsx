import React from 'react'
import { Button } from '../ui/button'
import {  Download, Eye, ReceiptText } from 'lucide-react'
import { Card, CardContent, CardHeader, CardTitle } from '../ui/card'
import { Badge } from '../ui/badge'
import { Table, TableBody, TableCell, TableRow } from '../ui/table'
import { Heading } from '../ui/heading'

type Props = {}
 const applicationData = {
    applicantName: "John Doe",
    cnic: "12345-6789012-3",
    formNumber: "FORM-2023-12345",
    challanNumber: "CHAL-2023-67890",
    challanStatus: "Unpaid",
    
  }
const title = "Challan and Application Form";
const description =
  "Download and Submit Challan after confirming challan. You will be allowed to download Application form";
const Challan = (props: Props) => {
  return (
     <>
      <Heading title={title} description={description} />
     <div className="min-h-screen mt-4 ">
      <Card className="w-full  mx-auto bg-white">
        <CardHeader className="border-b">
          <CardTitle className="text-2xl font-bold  text-gray-800">Application Details</CardTitle>
        </CardHeader>
        <CardContent className="p-0">
          <Table>
            <TableBody>
              {Object.entries(applicationData).map(([key, value]) => (
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
        </CardContent>
        <div className="p-6 bg-gray-50 border-t flex flex-col sm:flex-row justify-end space-y-2 sm:space-y-0 sm:space-x-2">
          <Button className="w-full sm:w-auto">
            <ReceiptText className="mr-2 h-4 w-4" /> Generate Challan
          </Button>
          <Button variant="outline" className="w-full sm:w-auto">
            <Download className="mr-2 h-4 w-4" /> Download  Form
          </Button>
        </div>
      </Card>
    </div>
    
     </>
  )
}
// IF application not found
//   <CardContent className="p-6">
//               <Alert variant="warning" className="mb-6">
//                 <AlertCircle className="h-4 w-4" />
//                 <AlertTitle>Application Not Filled</AlertTitle>
//                 <AlertDescription>
//                   You haven't filled out your application form yet. Please complete the form to proceed.
//                 </AlertDescription>
//               </Alert>
//               <Button className="w-full">
//                 <PenLine className="mr-2 h-4 w-4" /> Fill Application Form
//               </Button>
//             </CardContent>
//           )}

export default Challan