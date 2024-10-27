import React, { FC, useEffect, useMemo, useState } from "react";
import { Card } from "../../ui/card";
import { useFieldArray, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  degreeValidator,
  DegreeValues,
} from "@/lib/SchemaValidators/ApplicationForm/DegreeSchema.validator";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "../../ui/accordion";
import {  AlertTriangleIcon, ArrowLeft, ArrowRight, GraduationCap, Plus, Save } from "lucide-react";
import { cn } from "@/lib/utils";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../ui/form";
import { Input } from "../../ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../ui/select";
import { Button } from "../../ui/button";
import { useAppDispatch } from "@/hooks/reduxHooks";
import { nextStep, prevStep } from "@/redux/features/applicant/applicationWizardSlice";
import { IDegreeGroupWithDegreeLevel } from "@/types/degreeGroup";
import { useCreateApplicantDegreesMutation } from "@/redux/features/applicant/applicantApi";
import { toast } from "@/components/ui/use-toast";
import { BiSolidSave } from "react-icons/bi";

type CreateDegreeProps = {
appliedProgram: string;
 degreeList: any,
 degreeGroupData: IDegreeGroupWithDegreeLevel
}

const CreateDegree: FC<CreateDegreeProps> = ({appliedProgram,degreeList,degreeGroupData}) => {
    const dispatch = useAppDispatch();
    const [createApplicantDegrees,{isLoading : createIsLoading,isSuccess: createIsSuccess}] =  useCreateApplicantDegreesMutation()
    useEffect(() => {
    if (createIsSuccess) {
        toast({title: "Success",description: "Degrees saved successfully!"})
        dispatch(nextStep())
    }
    }, [createIsSuccess])
    const form = useForm<DegreeValues>({
    resolver: zodResolver(degreeValidator),
    defaultValues: {
      degrees: Array.from({ length: degreeList.length }, () => ({})),   },
    mode: "all",
  });
  const { control,formState:{errors} } = form;
  const { fields,append,remove } = useFieldArray({
    control,
    name: "degrees",
  });
  const processDegreeInfo =async (data: DegreeValues) => {
    console.log("data => ",data)
    await createApplicantDegrees(data);
  }
  return (
    <>
    <Card >
        <Form {...form}>
          <form onSubmit={form.handleSubmit(processDegreeInfo,(error) => console.log(error))}>
            {fields?.map((field, index) => (
              <div key={"degreeInfo" + index} className="first:pt-5 last:pb-5 px-3 pb-3">
                <Card className="col-span-3 px-3 shadow-none">
                  <Accordion
                    type="single"
                    collapsible
                    defaultValue={`gInfo` + index}
                  >
                    <AccordionItem
                      value={`gInfo` + index}
                      className="!border-b-0"
                    >
                      <AccordionTrigger
                        className={cn(
                          "[&[data-state=closed]>button]:hidden [&[data-state=open]>.alert]:hidden relative !no-underline py-3",
                          errors.degrees?.[index] && "text-red-700"
                        )}
                      >
                        <span className="flex items-center">
                          <GraduationCap className="h-[17px]  inline mr-0.5" />
                          <p>{typeof degreeList[index] ==  "undefined" ? "other" : degreeList[index]}</p>
                        </span>
                        { errors.degrees?.[index] && (
                            <span className="absolute alert right-8">
                              <AlertTriangleIcon className="h-4 w-4  text-red-700" />
                            </span>
                          )}
                      </AccordionTrigger>
                      <AccordionContent>
                     <div className=" space-y-2 border p-4 rounded-md">
            <div className={cn( "sm:grid gap-4 rounded-md relative",typeof degreeList[index] ==  "undefined" ? "md:grid-cols-2" : "md:grid-cols-3")}
                        >
                          <FormField
                            control={form.control}
                            name={`degrees.${index}.boardOrUniversityName`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Board or University</FormLabel>
                                <FormControl>
                                  <Input
                                    type="text"
                                    //   disabled={loading}
                                    {...field}
                                    error={errors.degrees?.[index]?.boardOrUniversityName?.message}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                           <FormField
                            control={form.control}
                            name={`degrees.${index}.degreeGroupId`}
                            render={({ field }) => (
                              <FormItem className={cn(`mb-2`,typeof degreeList[index] ==  "undefined" && "hidden")}>
                                <FormLabel>Group</FormLabel>
                                <Select
                                  //   disabled={loading}
                                  onValueChange={field.onChange}
                                  value={field.value}
                                  defaultValue={field.value}
                                >
                                  <FormControl>
                                    <SelectTrigger
                                     error={errors.degrees?.[index]?.degreeGroupId?.message}
                                    >
                                      <SelectValue
                                        defaultValue={field.value}
                                        placeholder="Select a Group"
                                      />
                                    </SelectTrigger>
                                  </FormControl>
                                  <SelectContent>
                                    {/* @ts-ignore  */}
                                   {degreeGroupData[degreeList[index]]?.map((value,i) => (<SelectItem key={value.degreeName} value={String(value.id)}>{value.degreeName} </SelectItem>) )}
                                  </SelectContent>
                                </Select>
                                <FormMessage />
                              </FormItem>
                            )}
                          />

                       
                        
                          <FormField
                            control={form.control}
                            name={`degrees.${index}.subject`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Subject</FormLabel>
                                <FormControl>
                                  <Input
                                   error={errors.degrees?.[index]?.subject?.message}
                                    placeholder="Subject"
                                    type="text"
                                    //   disabled={loading}
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                         
                        </div>
                        <div className="sm:grid sm:grid-cols-2 md:grid-cols-4 gap-4 rounded-md relative">
                             <FormField
                            control={form.control}
                            name={`degrees.${index}.rollNo`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Roll No</FormLabel>
                                <FormControl>
                                  <Input
                                  error={errors.degrees?.[index]?.rollNo?.message}
                                    type="text"
                                    //   disabled={loading}
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name={`degrees.${index}.passingYear`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Passing Year</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder=""
                                    error={errors.degrees?.[index]?.passingYear?.message}
                                    type="number"
                                    //   disabled={loading}
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />

                          <FormField
                            control={form.control}
                            name={`degrees.${index}.totalMarks`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Total Marks</FormLabel>
                                <FormControl>
                                  <Input
                                  error={errors.degrees?.[index]?.totalMarks?.message}
                                    placeholder=""
                                    type="number"
                                    //   disabled={loading}
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={form.control}
                            name={`degrees.${index}.obtainedMarks`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Obtained Marks</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder=""
                            error={errors.degrees?.[index]?.obtainedMarks?.message}
                                    type="number"
                                    //   disabled={loading}
                                    {...field}
                                  />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                        </div>
                     </div>
                      </AccordionContent>
                    </AccordionItem>
                  </Accordion>
                </Card>
              </div>
            ))}
             <div className="mt-4 flex justify-center">
                  <Button
                    type="button"
                    className="flex justify-center"
                    size={'sm'}
                    onClick={() =>
                      append({
                        degreeGroupId: 1002,
                        subject: "",
                        boardOrUniversityName: "",
                        rollNo: "",
                        passingYear: 0,
                        totalMarks:0,
                        obtainedMarks: 0
                      })
                    }
                  > <Plus className="size-4 mr-1" />
                    Add More
                  </Button>
                </div>
            <div className="flex justify-between mb-3 px-3">
                 <Button disabled={createIsLoading}  onClick={(e) => dispatch(prevStep())}>
                <ArrowLeft  className="size-4 mr-1" />
                Previous
              </Button>     
                <Button disabled={createIsLoading}  type="submit">
                <Save className="size-4 mr-1" />
                Save & Next
              </Button>
       </div>
          </form>
        </Form>
      </Card>
    </>
  )
}

export default CreateDegree