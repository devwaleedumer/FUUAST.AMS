import React, { FC, useEffect, useState } from "react";
import { Card, CardContent } from "../../ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../ui/form";
import { useFieldArray, useForm } from "react-hook-form";
import {
  admissionSelectionValidator,
  admissionSelectionValues,
} from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "../../ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../ui/select";
import { ArrowLeft, CheckCircle2, Info, Plus, SaveAll, TriangleAlert } from "lucide-react";
import { useGetAllFacultiesQuery, useLazyGetDepartmentsByFacultyIdQuery } from "@/redux/features/faculity/faculityApi";
import PageLoader from "../../shared/Loader";
import { useLazyGetTimeShiftByDepartmentIdQuery } from "@/redux/features/department/departmentApi";
import { Checkbox } from "../../ui/checkbox";
import { useSubmitApplicationFormMutation } from "@/redux/features/applicationForm/applicationFormApi";
import { toast } from "@/components/ui/use-toast";
import { Faculty } from "@/types/faculty";
import  confetti from "canvas-confetti"
import { useRouter } from "next/navigation";
import { useAppDispatch } from "@/hooks/reduxHooks";
import { prevStep } from "@/redux/features/applicant/applicationWizardSlice";
import List from "@/components/shared/List";
import { importantNotesAdmissionSelection } from "@/lib/data";

type CreateAdmissionSelectionsInfoProps = {
  facultyData:Faculty [],
  programId: number
};
const CreateAdmissionSelectionsInfo: FC<CreateAdmissionSelectionsInfoProps> = ({facultyData,programId}) => {
  const [departmentByFaculty,{isLoading: departmentByFacultyIsLoading}] = useLazyGetDepartmentsByFacultyIdQuery()
  const [timeShiftByDepartmentIdAndProgramId,{isLoading: timeShiftIsLoading}] =  useLazyGetTimeShiftByDepartmentIdQuery()
  const [submitApplicationForm,{isLoading: submitApplicationFormIsLoading,isSuccess: submitApplicationFormIsSuccess}] = useSubmitApplicationFormMutation()
   // Track department and shift data for each row
  const [departments, setDepartments] = useState<(any[] | undefined)[]>([]);
  const [timeShifts, setTimeShifts] = useState<(any[] | undefined)[]>([]);
  const router = useRouter();
  const handleFacultyChange = async (index: number,facultyId : number, programId: number) => {
    const {data}  = await  departmentByFaculty({facultyId,programId});
    const departmentList = [...departments];
    departmentList[index] = data;
    setDepartments(departmentList);
  }

    const handleDepartmentChange = async (index: number, programId: number, departmentId: number) => {
    const {data} = await timeShiftByDepartmentIdAndProgramId({programId,departmentId})
    const timeShiftList = [...timeShifts];
    timeShiftList[index] = data
    setTimeShifts(timeShiftList)
  }
  const dispatch = useAppDispatch()
  const handleNext = () => {
    dispatch(prevStep())
  }
  const form = useForm<admissionSelectionValues>({
    resolver: zodResolver(admissionSelectionValidator),
    defaultValues: {
      programsApplied: Array.from({ length:  1 }, () => ({})),
    },
    mode: "all",
  });
  const {
    control,
    formState: { errors },
    setValue
  } = form;
  const { append, remove, fields } = useFieldArray({
    control,
    name: "programsApplied",
  });
  const [isMounted, setIsMounted] = useState<boolean>(false);
    useEffect(() => {setIsMounted(true)},[])

  const processDegreeInfo = async (data: admissionSelectionValues) => {
    console.log("data => ", data);
    await submitApplicationForm(data).unwrap().then((data) => {
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
     confetti({
      particleCount: 100,
      spread: 70,
      origin: { y: 0.6 }
    })
    setTimeout(() => {
       router.push("/dashboard")
    },2000)
  })
  };
  return (
    <>
      {!isMounted ?
      <PageLoader/>
      :
      <Card >
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(processDegreeInfo)}>
              <div className="bg-blue-50 p-4 my-6 rounded text-sm text-blue-800 md:flex gap-0.5 flex-col items-center  justify-center ">
                <strong className="flex gap-[2px] justify-center items-center">
                  <Info className="size-[17px]" />  <p>IMPORTANT:</p>
                </strong>{" "}
                          <List className="p-5 self-start" list={importantNotesAdmissionSelection} title="Important notes before selecting programs." />

              </div>
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
              <div className="border rounded-lg p-4">
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
                          async   (value) => 
                            {
                             await handleFacultyChange(index,Number(value),programId);
                              setValue(`programsApplied.${index}.departmentId`,'' as any)
                              setValue(`programsApplied.${index}.timeShiftId`,'' as any)
                              field.onChange(value)

                            }}
                            value={field.value}
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
                                  placeholder="Select a Faculty"
                                />
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
                           await  handleDepartmentChange(index,Number(programId),Number(value))
                              setValue(`programsApplied.${index}.timeShiftId`,'' as any)
                              field.onChange(value)
                            }}
                            value={field.value}
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
                            value={field.value}
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
              {
            [1,2,3].includes(programId) &&  <div className="mt-4 flex justify-center">
                  <Button
                    type="button"
                    className="flex justify-center"
                    size={'sm'}
                    onClick={() =>
                      append({} as any)
                    }
                  >
                  <Plus className="size-4 mr-1" />  Add More
                  </Button>
                  </div>

              }
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
                <div className="mt-6 flex items-center justify-between">
                  <Button  type="button" onClick={handleNext}   disabled={submitApplicationFormIsLoading}>
                    <ArrowLeft className="mr-1 h-4 w-4" /> Back
                  </Button>
                  <Button type="submit"  disabled={submitApplicationFormIsLoading}>
                    <SaveAll className="mr-1 h-4 w-4" /> SUBMIT FORM
                  </Button>
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
// Todo

export default CreateAdmissionSelectionsInfo;
