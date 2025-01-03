import { Card, CardContent } from '@/components/ui/card';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { toast } from '@/components/ui/use-toast';
import { editAdmissionSelectionValidator, editAdmissionSelectionValues } from '@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator';
import { useEditSubmittedApplicationMutation } from '@/redux/features/applicationForm/applicationFormApi';
import { useLazyGetTimeShiftByDepartmentIdQuery } from '@/redux/features/department/departmentApi';
import { useLazyGetDepartmentsByFacultyIdQuery } from '@/redux/features/faculity/faculityApi';
import { zodResolver } from '@hookform/resolvers/zod';
import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from '@/components/ui/select';
import { ArrowLeft, CheckCircle2, FileText, Info, SaveAll, TriangleAlert } from 'lucide-react';
import React, { FC, useEffect, useState } from 'react'
import PageLoader from "../../shared/Loader";

import { useForm, useFieldArray } from 'react-hook-form';
import { Checkbox } from '@/components/ui/checkbox';
import { Faculty } from '@/types/faculty';
import { Button } from '@/components/ui/button';
import { SubmitApplicationFormResponse } from '@/types/applicationForm';
import { useAppDispatch } from '@/hooks/reduxHooks';
import { prevStep } from '@/redux/features/applicant/applicationWizardSlice';
import List from '@/components/shared/List';
import { importantNotesAdmissionSelection } from '@/lib/data';

type EditAdmissionSelectionInfoProps = {
    facultyData:Faculty [],
    data: SubmitApplicationFormResponse
}

const EditAdmissionSelectionInfo:FC<EditAdmissionSelectionInfoProps> = ({facultyData,data}) => {

  const [departmentByFaculty,{isLoading: departmentByFacultyIsLoading}] = useLazyGetDepartmentsByFacultyIdQuery()
  const [timeShiftByDepartmentIdAndProgramId,{isLoading: timeShiftIsLoading}] =  useLazyGetTimeShiftByDepartmentIdQuery()
  const [editApplicationForm,{isLoading: editApplicationFormIsLoading,isSuccess: editApplicationFormIsSuccess}] = useEditSubmittedApplicationMutation()
   // Track department and shift data for each row
  const [departments, setDepartments] = useState<(any[] | undefined)[]>(data.departments);
  const [timeShifts, setTimeShifts] = useState<(any[] | undefined)[]>(data.shifts);
  const dispatch = useAppDispatch()
    const handleNext = () => {
    dispatch(prevStep())
  }
  const form = useForm<editAdmissionSelectionValues>({
    resolver: zodResolver(editAdmissionSelectionValidator),
    values: data as any,
    mode: "all"
  });
  const {
    control,
    formState: { errors },
    setValue
  } = form;
  const {  fields } = useFieldArray({
    control,
    name: "programsApplied",
  });

  const handleFacultyChange = async (index: number,facultyId : number, programId: number) => {
    const {data}  = await  departmentByFaculty({programId,facultyId });
    const departmentList = [...departments];
    departmentList[index] = data;
    setDepartments(departmentList)
    setValue(`programsApplied.${index}.departmentId`,'' as any)
    setValue(`programsApplied.${index}.timeShiftId`,'' as any);
} 

    const handleDepartmentChange = async (index: number, programId: number, departmentId: number) => {
    const {data} = await timeShiftByDepartmentIdAndProgramId({programId,departmentId})
    const timeShiftList = [...timeShifts];
    timeShiftList[index] = data
    setTimeShifts(timeShiftList)
    setValue(`programsApplied.${index}.timeShiftId`,'' as any)

  }
  
  const processDegreeInfo = async (data: editAdmissionSelectionValues) => {
    console.log("data => ", data);
    await editApplicationForm(data).unwrap().then((data) => {
         toast({
      title: "Application Updated 🎓!",
      description: data,
      action: (
        <div className="flex items-center">
          <CheckCircle2 className="mr-2 h-5 w-5 text-green-500" />
          <span className="font-semibold text-green-500">Success</span>
        </div>
      ),
      className: "bg-white dark:bg-gray-800 border-green-500 border-2",
      duration: 5000,
    })
    })

  };
  return (
    <>
      {!data ?
      <PageLoader/>
      :
      <Card >
        <CardContent>
             <div className="border  bg-white my-6 rounded text-sm  md:flex gap-0.5 flex-col items-center  justify-center ">
                {/* <strong className="flex gap-[2px] justify-center items-center">
                  <Info className="size-[17px]" />  <p>IMPORTANT:</p>
                </strong>{" "} */}
                          <List className="p-5 self-start" list={importantNotesAdmissionSelection} title="Important notes before selecting programs." />

              </div>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(processDegreeInfo)}>
              {/* <div className="bg-blue-100 p-4 my-6 rounded text-sm text-blue-800 md:flex gap-0.5  ">
                <strong className="flex gap-[2px]">
                  <Info className="size-[17px]" /> IMPORTANT:
                </strong>{" "}
                Order of choice of the disciplines once submitted Should not be
                changed under any circumstances.
              </div> */}
            <div className="border rounded-lg p-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4 ">
                <FormField
                  control={form.control}
                  name={`expelledFromUni`}
                  render={({ field }) => (
                    <FormItem className="mb-2">
                      <FormLabel>EVER EXPELLED FROM ANY UNIVERSITY?</FormLabel>
                      <Select
                        //   disabled={loading}
                        onValueChange={field.onChange}
                        value={field.value}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger
                            error={errors.expelledFromUni?.message}
                          >
                            <SelectValue
                              defaultValue={field.value}
                              placeholder="Select an option"
                            />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectItem value="yes">YES</SelectItem>
                          <SelectItem value="no">NO</SelectItem>
                        </SelectContent>
                      </Select>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name={`heardAboutUniFrom`}
                  render={({ field }) => (
                    <FormItem className="mb-2">
                      <FormLabel>HOW DID YOU KNOW ABOUT ADMISSION</FormLabel>
                      <Select
                        //   disabled={loading}
                        onValueChange={field.onChange}
                        value={field.value}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger error={errors.heardAboutUniFrom?.message}>
                            <SelectValue
                              defaultValue={field.value}
                              placeholder="Select an option"
                            />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectItem value="website">
                            University Website
                          </SelectItem>
                          <SelectItem value="friend">
                            Friend or Family
                          </SelectItem>
                          <SelectItem value="advertisement">
                            Advertisement
                          </SelectItem>
                        </SelectContent>
                      </Select>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <div>
                {fields.map((field, index) => (
                  <div
                    key={index}
                    className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4 items-end"
                  >
                    <div className="md:hidden text-lg font-bold text-primary mb-2 ">
                      Program Choice {String(index + 1).padStart(2, "0")}
                    </div>
                    <div className="hidden md:flex items-center justify-center">
                      <span className="text-lg font-bold text-primary">
                        {String(index + 1).padStart(2, "0")}.
                      </span>
                    </div>
                    <FormField
                      control={form.control}
                      name={`programsApplied.${index}.facultyId`}
                      render={({ field }) => (
                        <FormItem className="mb-2">
                          <FormLabel>Faculty</FormLabel>
                          <Select
                              // disabled={loading}
                            onValueChange={
                             async (value) => {
                             await handleFacultyChange(index,Number(value),data.programId);
                              field.onChange(value)
                            }}
                            value={String(field.value)}
                            defaultValue={field.value}
                          >
                            <FormControl>
                              <SelectTrigger
                                error={
                                  errors.programsApplied?.[index]?.facultyId
                                    ?.message
                                }
                              >
                                <SelectValue
                                  defaultValue={field.value}
                                  placeholder="Select a Faculty"/>
                              </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                              {facultyData?.map((value, index) => (
                                <SelectItem
                                  key={value.id}
                                  value={String(value.id)}
                                >
                                  {value.name}
                                </SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
                   <FormField
                      control={form.control}
                      key={index}
                      name={`programsApplied.${index}.departmentId`}
                      render={({ field }) => (
                        <FormItem className="mb-2">
                          <FormLabel>Department</FormLabel>
                          <Select
                             disabled={!departments[index]}
                            onValueChange={
                           async  (value) => 
                            {
                           await  handleDepartmentChange(index,Number(data.programId),Number(value))
                              field.onChange(Number(value))
                            }}
                            value={String(field.value)}
                            defaultValue={field.value}
                          >
                            <FormControl>
                              <SelectTrigger
                                error={
                                  errors.programsApplied?.[index]?.departmentId
                                    ?.message
                                }
                              >
                                <SelectValue
                                  defaultValue={field.value}
                                  placeholder="Select a department"
                                />
                              </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                              {/* <SelectItem value={"a"}>Select a department</SelectItem> */}
                              {departments?.[index]?.map((value,index) => (<SelectItem  key={value.id}value={String(value?.id)}>{value?.name} </SelectItem>))}                     
                            </SelectContent>
                          </Select>
                          <FormMessage />
                        </FormItem>
                      )}
                    />
               

                    <FormField
                      control={form.control}
                      name={`programsApplied.${index}.timeShiftId`}
                      render={({ field }) => (
                        <FormItem className="mb-2">
                          <FormLabel>Shift</FormLabel>
                          <Select
                            disabled={!timeShifts[index]}
                            onValueChange={field.onChange}
                            value={String(field.value)}
                            defaultValue={field.value}
                          >
                            <FormControl>
                              <SelectTrigger
                                error={
                                  errors.programsApplied?.[index]?.timeShiftId
                                    ?.message
                                }
                              >
                                <SelectValue
                                  defaultValue={field.value}
                                  placeholder="Select a Shift"
                                />
                              </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                              {timeShifts?.[index]?.map((value,index) => (<SelectItem
                            key={value.id}
                               value={String(value.id)}>{value.name}
                                </SelectItem>))}                     
                            </SelectContent>
                          </Select>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                  </div>
                ))}
           <FormField
                control={form.control}
                name="infoConsent"
                render={({ field }) => (
            <FormItem className="flex flex-row  justify-center items-center space-x-3 space-y-0 md:mt-6">
              <FormControl>
                <Checkbox
                  checked={field.value}
                  onCheckedChange={field.onChange}
                />
              </FormControl>
              <div className="space-y-1 leading-none">
                <FormLabel className="text-sm">
                  All information provided is correct. University have rights to cancel your application at any phase.
                </FormLabel>
                
              </div>
            </FormItem>
          )}
        />
               <div className="mt-6 flex items-center justify-between gap-x-2">
                  <Button onClick={handleNext}   className="" type='button'  disabled={editApplicationFormIsLoading}>
                    <ArrowLeft className="mr-1 h-4 w-4" /> Back
                  </Button>
                  <div  className='space-x-2 flex'>
                    {/* <Button  type="button">
                    <SaveAll className="mr-1 h-4 w-4"  />  Challan
                    </Button> */}
                    <Button type='submit'  disabled={editApplicationFormIsLoading}>
                    <FileText className="mr-1 h-4 w-4" /> Update
                  </Button>
                  </div>
                </div>
              </div>
            </div>
              
            </form>
          </Form>
          <div className=" mt-6 p-4 bg-red-100 rounded text-sm text-red-800 md:flex gap-0.5">
            <strong className="flex">
              {" "}
              <TriangleAlert className="size-4 mr-1" /> Attention:
            </strong>{" "}
            Make sure before submit once you submit the form you can&apos;t
            change the Name Field. Any false / incorrect information will lead
            to rejection of admission form.
          </div>
        </CardContent>
      </Card>
      }
    </>
  );
};

export default EditAdmissionSelectionInfo