import React, { FC, useEffect } from 'react'
import { Heading } from '../ui/heading'
import { Card, CardContent } from '../ui/card'
import { SubmitHandler, useForm } from 'react-hook-form'
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from '../ui/form'
import { programValidator,  ProgramValues } from '@/lib/SchemaValidators/ApplicationForm/ProgramSchema.validator'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '../ui/select'
import { Button } from '../ui/button'
import {  ArrowLeft, ArrowRight, Asterisk, SaveAll } from 'lucide-react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAppDispatch, useAppSelector } from '@/hooks/reduxHooks'
import { nextStep, prevStep } from '@/redux/features/applicant/applicationWizardSlice'
import PageLoader from '../shared/Loader'
import { getSelectedProgram, setSelectedProgram } from '@/lib/services/wizardLocalStorageService'
import {  useGetProgramByUserIdQuery } from '@/redux/features/applicant/applicantApi'
import { RootState } from '@/redux/store'
import { useGetAllProgramsQuery } from '@/redux/features/program/programApi'
import { useCreateApplicationFormMutation } from '@/redux/features/applicationForm/applicationFormApi'
import { toast } from '../ui/use-toast'
const title = "Program Selection"
const description = "Select a program in which you are interested"
type ProgramInfoProps = {
}
const ProgramInfo : FC<ProgramInfoProps> = ({}) => {
  const [createApplication, {isLoading: createApplicationLoading,isSuccess:createApplicationSuccess,data:createApplicationData}] = useCreateApplicationFormMutation();
  const id = useAppSelector((state : RootState) => state.auth?.user?.id)
  const {data: programData, isSuccess : programDataIsSuccess,isLoading : programDataLoading } = useGetProgramByUserIdQuery(Number(id))
  const dispatch = useAppDispatch();
  const form = useForm<ProgramValues>
  ({
    resolver: zodResolver(programValidator),
    mode:"all",
  });
  useEffect(() => {
    if(createApplicationSuccess){
      toast({
        title: "Success",
        description: "Program successfully selected"
      })   
              dispatch(nextStep());

      
    }
  
   
  }, [createApplicationSuccess])
  
  const {data, isLoading} = useGetAllProgramsQuery(null);
  const {formState: {errors}} = form
    const processProgramInfoForm: SubmitHandler<ProgramValues> =async (formData) => {
      if (programDataIsSuccess) {
        dispatch(nextStep());
        return;
      }
      await createApplication({ programId: Number(formData.programId) })

  };
 
  return (data == undefined && programData == undefined ? <PageLoader/> : 
    <>
   <Heading   title={title} description={description}/>
   <Card >
    <CardContent className='pt-4'>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(processProgramInfoForm)}>
            <FormField
                    control={form.control}
                    name="programId"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className='relative' >Program  <Asterisk className='size-2 inline-flex absolute top-[2px]'/> </FormLabel>
                        <FormDescription>Once program selected it cannot be changed later on</FormDescription>
                        <Select
                          onValueChange={field.onChange} 
                          value={(field.value) || String(programData?.id || "")}
                          disabled={programData != undefined || createApplicationSuccess}
                          >
                          <FormControl>
                            <SelectTrigger error={errors.programId?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a Program"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {data?.map((program) => (
                              <SelectItem
                                key={program.id + "programInfo"}
                                value={String(program.id) }
                              >
                                {program.name}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
              <div className=" flex justify-between mt-4">
               <Button  type="button" onClick={() => dispatch(prevStep())}>
                <ArrowLeft className="size-4 mr-1" />
                Previous
              </Button>
              <div className='flex'>
                <Button type="submit" disabled={createApplicationLoading}>
                    <SaveAll className="mr-1 size-4" /> Save & Next
                  </Button>
               
              </div>
              </div>
               </form>
      </Form>
    </CardContent>
   </Card>
</>


  )
}

export default ProgramInfo