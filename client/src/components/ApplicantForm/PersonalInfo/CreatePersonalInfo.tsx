/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @next/next/no-img-element */
"use client";
import { ProfileFileUpload } from "@/components/ApplicantForm/ProfileFileUpload";
import List from "@/components/shared/List";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Heading } from "@/components/ui/heading";
import { Input } from "@/components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { toast } from "@/components/ui/use-toast";
import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks";
import {
  bloodGroups,
  genders,
  religions,
  photographRequirements,
} from "@/lib/data";
import {
  PersonalEditInfoValues,
  personalInfo,
  personalInfoDefaults,
  personalInfoEditSchema,
  PersonalInfoValues,
} from "@/lib/SchemaValidators/ApplicationForm/PersonalInfoSchema.validator";
import { cn, omitProps } from "@/lib/utils";
import {
  useCreateApplicantPersonalInformationMutation,
  useGetApplicantPersonalInformationQuery,
} from "@/redux/features/applicant/applicantApi";
import {
  nextStep,
  prevStep,
} from "@/redux/features/applicant/applicationWizardSlice";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
import {
  AlertTriangleIcon,
  CalendarIcon,
  MapPinIcon,
  BookUser,
  UserRoundCheck,
  Save,
  ArrowRight,
  ArrowLeft,
  Asterisk,
} from "lucide-react";
import { FC, useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
const description =
  "For processing your application, we first following information about you.";
type CreatePersonalInfoProps = {
  // personalInformationData : PersonalInformation | undefined
};

const CreatePersonalInfo: FC<CreatePersonalInfoProps> = ({}) => {
  const dispatch = useAppDispatch();
  const [create, ] =
    useCreateApplicantPersonalInformationMutation();
//  useEffect(() => {
 
//   }, [isSuccess])  
  const form = useForm<PersonalInfoValues>({
    resolver: zodResolver( personalInfo ),
    mode: "all",
    defaultValues: personalInfoDefaults
  });
  const [loading, setLoading] = useState(false);
  const {
    setValue,
    formState: { errors },
  } = form;
  
  const processPersonalInfoForm: SubmitHandler<PersonalInfoValues> = async (
    formData
  ) => {
    // Dispatch and next step 
  await create(formData).unwrap()
                        .then((data) => {
                           toast({
                                  title: "Success",
                                  description:  `Applicant created with id ${data.id}`
                                })
                           dispatch(nextStep());
                        });
   
    
      };

  return <> <Heading
        title={"Step#1. Personal Information"}
        description={description}
      />
      <Card>
        <CardContent>
          <List className="p-5" list={photographRequirements} />
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(processPersonalInfoForm, (error) => {
                console.log(error);
              })}
              className="space-y-4 w-full"
            >
              <div className={cn("md:grid md:grid-cols-3 gap-x-3 ")}>
                <>
                    <div className="col-span-3">
                      <FormField
                        control={form.control}
                        name="profileImage"
                        render={({ field }) => (
                          <FormItem className="mb-2">
                            <FormLabel className="relative">
                              Profile{" "}
                              <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                            </FormLabel>
                            <FormControl>
                              <ProfileFileUpload
                                setValue={setValue}
                                onChange={field.onChange}
                                value={field.value}
                                isValid={!!errors.profileImage}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    </div>
                  

                  <FormField
                    control={form.control}
                    name="cnic"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          CNIC{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            placeholder="61101xxxxxxx"
                            {...field}
                            error={errors?.cnic?.message}
                            type="number"
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="mobileNo"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Contact No.{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            error={errors.mobileNo?.message}
                            placeholder="03xxxxxxxxx"
                            disabled={loading}
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="dob"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          DOB{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <Popover>
                          <PopoverTrigger asChild>
                            <FormControl>
                              <Button
                                variant={"outline"}
                                className={cn(
                                  " w-full  pl-3 text-left font-normal",
                                  !field.value && "text-muted-foreground"
                                )}
                              >
                                {field.value ? (
                                  format(field.value, "PPP")
                                ) : (
                                  <span>Pick a date</span>
                                )}
                                <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                              </Button>
                            </FormControl>
                          </PopoverTrigger>
                          <PopoverContent className="w-auto p-0" align="start">
                            <Calendar
                              mode="single"
                              selected={(field.value as any) !== typeof(Date) ? new Date(field.value) : field.value }
                              onSelect={field.onChange}
                              disabled={(date) =>
                                date > new Date() ||
                                date < new Date("1900-01-01")
                              }
                              initialFocus
                            />
                          </PopoverContent>
                        </Popover>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="fatherName"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Father Name{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            error={errors.fatherName?.message}
                            placeholder="Your father Name"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="gender"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Gender{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <Select
                          disabled={loading}
                          onValueChange={field.onChange}
                          value={field.value}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger error={errors.gender?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a gender"
                                className="!border-2 !border-blue-500"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {genders.map((city) => (
                              <SelectItem
                                key={city.id + "gender"}
                                value={city.id}
                              >
                                {city.name}
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
                    name="bloodGroup"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Blood group{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <Select
                          disabled={loading}
                          onValueChange={field.onChange}
                          value={field.value}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger error={errors.bloodGroup?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a Blood group"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {bloodGroups.map((bg) => (
                              <SelectItem
                                key={bg.id + "bloodGroup"}
                                value={bg.id}
                              >
                                {bg.name}
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
                    name="religion"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Religion{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <Select
                          disabled={loading}
                          onValueChange={field.onChange}
                          value={field.value}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger error={errors.religion?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a Religion"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {religions.map((rlgn) => (
                              <SelectItem
                                key={rlgn.id + "rlgn"}
                                value={rlgn.id}
                              >
                                {rlgn.name}
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
                    name="domicile"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Domicile{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            placeholder="Domicile"
                            error={errors.domicile?.message}
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="province"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Province{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            error={errors.province?.message}
                            placeholder="Province"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="postalCode"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Postal code{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            placeholder="Postal Code"
                            {...field}
                            type="number"
                            error={errors.postalCode?.message}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="city"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          City{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            placeholder="City"
                            {...field}
                            error={errors.city?.message}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="country"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Country{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <Input
                            disabled={loading}
                            placeholder="Country"
                            {...field}
                            error={errors.country?.message}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="permanentAddress"
                    render={({ field }) => (
                      <FormItem className="md:col-span-3 mb-2">
                        <FormLabel className="relative">
                          Permanent address{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <FormControl>
                          <div className="relative">
                            <Input
                              disabled={loading}
                              placeholder="Permanent Address"
                              {...field}
                              error={errors.permanentAddress?.message}
                            />
                            <MapPinIcon className="absolute right-3 top-2 text-zinc-300 font-light" />
                          </div>
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  {/* Guardian Accordion */}
                  <Card className="col-span-3 px-3 mt-2 shadow-none">
                    <Accordion type="single" collapsible defaultValue="gInfo">
                      <AccordionItem value="gInfo" className="border-b-0">
                        <AccordionTrigger
                          className={cn(
                            "[&[data-state=closed]>button]:hidden [&[data-state=open]>.alert]:hidden relative !no-underline py-3",
                            errors.guardian && "text-red-700"
                          )}
                        >
                          <span className="flex items-center">
                            <UserRoundCheck className="h-[17px]  inline mr-0.5" />
                            <p>Guardian Information</p>
                          </span>
                          {errors.guardian && (
                            <span className="absolute alert right-8">
                              <AlertTriangleIcon className="h-4 w-4  text-red-700" />
                            </span>
                          )}
                        </AccordionTrigger>
                        <AccordionContent>
                          <div
                            className={cn(
                              "md:grid md:grid-cols-2 gap-4 border p-4 rounded-md relative mb-4"
                            )}
                          >
                            <FormField
                              control={form.control}
                              name={`guardian.name`}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Guardian Name{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      type="text"
                                      error={errors.guardian?.name?.message}
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={`guardian.relation`}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Guardian relation{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder=""
                                      type="text"
                                      error={errors.guardian?.relation?.message}
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={"guardian.permanentAddress"}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Guardian address{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="Guardian Address"
                                      type="text"
                                      error={
                                        errors.guardian?.permanentAddress
                                          ?.message
                                      }
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={`guardian.contactNo`}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Guardian contact{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="03xxxxxxxxx"
                                      type="number"
                                      error={
                                        errors.guardian?.contactNo?.message
                                      }
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                          </div>
                        </AccordionContent>
                      </AccordionItem>
                    </Accordion>
                  </Card>

                  {/* Emergency Contact Accordion */}
                  <Card className="col-span-3 mt-2 shadow-none">
                    <Accordion type="single" collapsible className="px-3">
                      <AccordionItem className="border-b-0" value="ecInfo">
                        <AccordionTrigger
                          className={cn(
                            "[&[data-state=closed]>button]:hidden [&[data-state=open]>.alert]:hidden relative !no-underline py-3",
                            errors.emergencyContact && "text-red-700"
                          )}
                        >
                          <span className="flex items-center">
                            <BookUser className="h-[17px]  inline mr-0.5" />
                            <p>Emergency Contact Information</p>
                          </span>
                          {errors.emergencyContact && (
                            <span className="absolute alert right-8">
                              <AlertTriangleIcon className="h-4 w-4   text-red-700" />
                            </span>
                          )}
                        </AccordionTrigger>
                        <AccordionContent>
                          <div
                            className={cn(
                              "md:grid md:grid-cols-2 gap-8 border p-4 rounded-md relative mb-4"
                            )}
                          >
                            <FormField
                              control={form.control}
                              name={"emergencyContact.name"}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Contact name{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="Contact Name"
                                      error={
                                        errors.emergencyContact?.name?.message
                                      }
                                      type="text"
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={"emergencyContact.relation"}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Contact relation{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="Relation"
                                      type="text"
                                      error={
                                        errors.emergencyContact?.relation
                                          ?.message
                                      }
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={"emergencyContact.permanentAddress"}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Contact address{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="Address"
                                      type="text"
                                      error={
                                        errors.emergencyContact
                                          ?.permanentAddress?.message
                                      }
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name={"emergencyContact.contactNo"}
                              render={({ field }) => (
                                <FormItem>
                                  <FormLabel className="relative">
                                    Contact no.{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <FormControl>
                                    <Input
                                      placeholder="03xxxxxxxxx"
                                      type="number"
                                      error={
                                        errors.emergencyContact?.contactNo
                                          ?.message
                                      }
                                      disabled={loading}
                                      {...field}
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                          </div>
                        </AccordionContent>
                      </AccordionItem>
                    </Accordion>
                  </Card>
                </>
              </div>
              <div className=" flex justify-end w-full space-x-4">
                <Button disabled={loading} type="submit">
                  <Save className="size-4 mr-1" />
                  Save & Next
                </Button>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </>
 
};

export default CreatePersonalInfo;
