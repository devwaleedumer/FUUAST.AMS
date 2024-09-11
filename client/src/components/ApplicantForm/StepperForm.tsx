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
import { Card, CardContent, CardDescription, CardHeader } from "@/components/ui/card";
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
import {
  bloodGroups,
  genders,
  religions,
  photographRequirements,
} from "@/lib/data";
import { personalInfo, PersonalInfoValues } from "@/lib/SchemaValidators/ApplicationForm/PersonalInfoSchema.Validator";
import { cn } from "@/lib/utils";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
import {
  AlertTriangleIcon,
  CalendarIcon,
  MapPinIcon,
  BookUser,
  UserRoundCheck,
  Save,
  GraduationCap,
} from "lucide-react";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { SubmitHandler, useFieldArray, useForm } from "react-hook-form";

interface ProfileFormType {
  initialData: any | null;
  categories: any;
}

export const Application: React.FC<ProfileFormType> = ({
  initialData,
  categories,
}) => {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  // const title = initialData ? "Edit Information" : "Admission form";
  const description =
    "For processing your application, we first following information about you.";
  const toastMessage = initialData
    ? "Information updated."
    : "Application Submitted.";
  const action = initialData ? "Save changes" : "Submit Form";

  const defaultValues = {
    degrees: [
      {
        degreeGroupId: '',
        subject: '',
        boardOrUniversity: '',
        passingYear: '',
        rollNo: '',
        totalMarks:'',
        obtainedMarks: '',
      }
    ],
  };

  const form = useForm<PersonalInfoValues>({
    resolver: zodResolver(personalInfo),
    mode: "all",
  });

  const {
    formState: { errors },
  } = form;



  const guardianAccordionError =
    errors?.guardianName !== undefined ||
    errors?.guardianContact !== undefined ||
    errors?.guardianRelation !== undefined ||
    errors?.guardianPermanentAddress !== undefined;
  const emergencyCAccordionError =
    errors?.emergencyCName !== undefined ||
    errors?.emergencyCContact !== undefined ||
    errors?.emergencyCRelation !== undefined;
  errors?.emergencyCPermanentAddress !== undefined;

  const processForm: SubmitHandler<PersonalInfoValues> = (data) => {
    console.log("data ==>", data);
    // api call and reset
    // form.reset();
  };

  type FieldName = keyof PersonalInfoValues;

  const steps = [
    {
      id: "Step 1",
      name: "Personal Information",
      fields: [
        "profileImage",
        "contactNo",
        "cnic",
        "isFatherDeceased",
        "fatherName",
        "gender",
        "bloodGroup",
        "religion",
        "disabilityDetail",
        "domicile",
        "province",
        "country",
        "city",
        "postalCode",
        "permanentAddress",
        "guardianName",
        "guardianRelation",
        "guardianContact",
        "guardianOccupation",
        "emergencyCPermanentAddress",
        "emergencyCName",
        "emergencyCRelation",
        "emergencyCContact",
        "guardianPermanentAddress",
      ],
    },
    {
      id: "Step 2",
      name: "Previous Degrees",
      // fields are mapping and flattening for the error to be trigger  for the dynamic fields
      // fields: degreeFields
      //   ?.map((_, index) => [
      //     `degrees.${index}.degreeGroup`,
      //     `degrees.${index}.subject`,
      //     `degrees.${index}.degreeMajor`,
      //     `degrees.${index}.obtainedMarks`,
      //     `degrees.${index}.totalMarks`,
      //     `degrees.${index}.passingYear`,
      //     `degrees.${index}.endingYear`,
      //     // Add other field names as needed
      //   ])
      //   .flat(),
    }
  ];

  const next = async () => {
    // triggers field of that step
    const fields = steps[currentStep].fields;

    const output = await form.trigger(fields as FieldName[], {
      shouldFocus: true,
    });

    if (!output) return;

    if (currentStep < steps.length - 1) {
      if (currentStep === steps.length - 2) {
        await form.handleSubmit(processForm)();
      }
      setPreviousStep(currentStep);
      setCurrentStep((step) => step + 1);
    }
  };

  const prev = () => {
    if (currentStep > 0) {
      setPreviousStep(currentStep);
      setCurrentStep((step) => step - 1);
    }
  };

  return (
    <>
      <Heading title={"Personal Information"} description={description} />
      <Card >
      <CardContent>
          <List className="p-5" list={photographRequirements} />
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(processForm)}
            className="space-y-8 w-full"
          >
            <div className={cn("md:grid md:grid-cols-3 gap-x-3 ")}>
              <>
                <div className="col-span-3">
                  <FormField
                    control={form.control}
                    name="profileImage"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel>Profile</FormLabel>
                        <FormControl>
                          <ProfileFileUpload
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
                      <FormLabel>CNIC</FormLabel>
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
                  name="contactNo"
                  render={({ field }) => (
                    <FormItem className="mb-2">
                      <FormLabel>Contact Number</FormLabel>
                      <FormControl>
                        <Input
                          type="number"
                          error={errors.contactNo?.message}
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
                      <FormLabel>Date of birth</FormLabel>
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
                            selected={field.value as any}
                            onSelect={field.onChange}
                            disabled={(date) =>
                              date > new Date() || date < new Date("1900-01-01")
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
                      <FormLabel>Father Name</FormLabel>
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
                      <FormLabel>Gender</FormLabel>
                      <Select
                        disabled={loading}
                        onValueChange={field.onChange}
                        value={field.value}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
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
                      <FormLabel>Blood group</FormLabel>
                      <Select
                        disabled={loading}
                        onValueChange={field.onChange}
                        value={field.value}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
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
                      <FormLabel>Religion</FormLabel>
                      <Select
                        disabled={loading}
                        onValueChange={field.onChange}
                        value={field.value}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
                            <SelectValue
                              defaultValue={field.value}
                              placeholder="Select a Religion"
                            />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          {/* @ts-ignore  */}
                          {religions.map((rlgn) => (
                            <SelectItem key={rlgn.id + "rlgn"} value={rlgn.id}>
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
                      <FormLabel>Domicile</FormLabel>
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
                      <FormLabel>Province</FormLabel>
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
                      <FormLabel>Postal Code</FormLabel>
                      <FormControl>
                        <Input
                          disabled={loading}
                          placeholder="Postal Code"
                          {...field}
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
                      <FormLabel>City</FormLabel>
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
                      <FormLabel>Country</FormLabel>
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
                      <FormLabel>Permanent Address</FormLabel>
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
                          guardianAccordionError && "text-red-700"
                        )}
                      >
                        <span  className="flex items-center">
                          <UserRoundCheck className="h-[17px]  inline mr-0.5" />
                          <p>
                            Guardian Information
                          </p>
                        </span>
                        {guardianAccordionError && (
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
                            name={`guardianName`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Guardian Name</FormLabel>
                                <FormControl>
                                  <Input
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
                            name={`guardianRelation`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Guardian Relation</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder=""
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
                            name={"guardianPermanentAddress"}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Guardian Address</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder="Address"
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
                            name={`guardianContact`}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Guardian Contact</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder="03xxxxxxxxx"
                                    type="number"
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
                          emergencyCAccordionError && "text-red-700"
                        )}
                      >
                        <span  className="flex items-center">
                          <BookUser className="h-[17px]  inline mr-0.5" />
                          <p>
                          Emergency Contact Information
                          </p>
                        </span>
                        {emergencyCAccordionError && (
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
                            name={"emergencyCName"}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Contact Name</FormLabel>
                                <FormControl>
                                  <Input
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
                            name={"emergencyCRelation"}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Contact Relation</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder="Relation"
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
                            name={"emergencyCPermanentAddress"}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Contact Address</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder="Address"
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
                            name={"emergencyCContact"}
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Contact No.</FormLabel>
                                <FormControl>
                                  <Input
                                    placeholder="03xxxxxxxxx"
                                    type="number"
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
             <Button disabled={loading}   type="submit">
            <Save className="size-4 mr-1"/>
            {action}
          </Button>
          </form>
        </Form>
      </CardContent>
      </Card>
  
    </>
  );
};
