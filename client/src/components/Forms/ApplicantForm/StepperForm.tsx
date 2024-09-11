"use client";
import { ProfileFileUpload } from "@/components/Forms/ApplicantForm/ProfileFileUpload";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
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
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Separator } from "@/components/ui/separator";
import {
  profileSchema,
  type ProfileFormValues,
} from "@/lib/SchemaValidators/ApplicationForm/FormSchema.Validator";
import {
  cities,
  countries,
  bloodGroups,
  genders,
  religions,
  degreeLevel,
  examType,
  gradingType,
} from "@/lib/data";
import { cn } from "@/lib/utils";
import { zodResolver } from "@hookform/resolvers/zod";
import { AlertTriangleIcon, Trash, Trash2Icon } from "lucide-react";
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
  const params = useParams();
  const router = useRouter();
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  // const title = initialData ? "Edit Information" : "Admission form";
  const description = "For processing your application, we first following information about you.";
  const toastMessage = initialData ? "Information updated." : "Application Submitted.";
  const action = initialData ? "Save changes" : "Create";
  const [previousStep, setPreviousStep] = useState(0);
  const [currentStep, setCurrentStep] = useState(0);
  const [data, setData] = useState({});
  const delta = currentStep - previousStep;
  const [isMounted, setIsMounted] = useState<boolean>(false)
  useEffect(() => {
    setIsMounted(true);
  }, [])

  const defaultValues = {
    // addresses: [
    //   {

    //     addressId: 1,
    //   },
    //   {
    //     addressId: 2
    //   }
    // ]
    // ,
    degrees: [
      {
        degreeName: "",
        degreeMajor: "",
        degreeLevel: "",
        boardOrUniversity: "",
        institution: "",

      },
    ],
  };

  const form = useForm<ProfileFormValues>({
    resolver: zodResolver(profileSchema),
    defaultValues,
    mode: "all",
  });

  const {
    control,
    getValues,
    setValue,
    watch,
    formState: { errors },
  } = form;
  const { append, remove, fields: degreeFields } = useFieldArray({
    control,
    name: "degrees",
  });

  // const { fields: addressesFields } = useFieldArray({
  //   control,
  //   name: "addresses",
  // });




  const guardianAccordionError =
    errors?.guardianName !== undefined ||
    errors?.guardianContact !== undefined ||
    errors?.guardianRelation !== undefined ||
    errors?.permanentAddress !== undefined;
  const emergencyCAccordionError =
    errors?.emergencyCName !== undefined ||
    errors?.emergencyCContact !== undefined ||
    errors?.emergencyCRelation !== undefined;
    errors?.permanentAddress !== undefined;
  const onSubmit = async (data: ProfileFormValues) => {
    try {
      setLoading(true);
      if (initialData) {
        // await axios.post(`/api/products/edit-product/${initialData._id}`, data);
      } else {
        // const res = await axios.post(`/api/products/create-product`, data);
        // console.log("product", res);
      }
      router.refresh();
      router.push(`/products`);
    } catch (error: any) {
    } finally {
      setLoading(false);
    }
  };

  const onDelete = async () => {
    try {
      setLoading(true);
      //   await axios.delete(`/api/${params.storeId}/products/${params.productId}`);
      router.refresh();
      router.push(`/${params.storeId}/products`);
    } catch (error: any) {
    } finally {
      setLoading(false);
      setOpen(false);
    }
  };

  const processForm: SubmitHandler<ProfileFormValues> = (data) => {
    console.log("data ==>", data);
    setData(data);
    // api call and reset
    // form.reset();
  };

  type FieldName = keyof ProfileFormValues;

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
        // "motherName",
        // "fatherOccupation",
        // "fatherContact",
        // "nextOfKinName",
        // "nextOfKinRelation",
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
        "guardianPermanentAddress"
        // ...addressesFields.map((_, index) => [
        //   `addresses.${index}.streetAddress`,
        //   `addresses.${index}.addressProvince`,
        //   `addresses.${index}.addressDistrict`,
        //   `addresses.${index}.addressPostalCode`,
        //   `addresses.${index}.addressId`,
        // ]).flat()

      ],
    },
    {
      id: "Step 2",
      name: "Previous Degrees",
      // fields are mapping and flattening for the error to be trigger  for the dynamic fields
      fields: degreeFields
        ?.map((_, index) => [
          `degrees.${index}.degreeLevel`,
          `degrees.${index}.degreeName`,
          `degrees.${index}.degreeMajor`,
          `degrees.${index}.boardOrUniversity`,
          `degrees.${index}.institution`,
          `degrees.${index}.gradingType`,
          `degrees.${index}.examType`,
          `degrees.${index}.obtainedMarks`,
          `degrees.${index}.totalMarks`,
          `degrees.${index}.startingYear`,
          `degrees.${index}.endingYear`,
          // Add other field names as needed
        ])
        .flat(),
    },
    { id: "Step 3", name: "Program Selection" },
    { id: "Step 4", name: "Generate Challan" },
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

  return (isMounted && (<>
    <div className="flex items-center justify-between">
      <Heading title={"Personal Information"} description={description} />
      {initialData && (
        <Button
          disabled={loading}
          variant="destructive"
          size="sm"
          onClick={() => setOpen(true)}
        >
          <Trash className="h-4 w-4" />
        </Button>
      )}
    </div>
    {/* <Separator /> */}
    {/* <div>
      <ul className="flex gap-4">
        {steps.map((step, index) => (
          <li key={step.name} className="md:flex-1">
            {currentStep > index ? (
              <div className="group flex w-full flex-col border-l-4 border-primary py-2 pl-4 transition-colors md:border-l-0 md:border-t-4 md:pb-0 md:pl-0 md:pt-4">
                <span className="text-sm font-medium text-primary transition-colors ">
                  {step.id}
                </span>
                <span className="text-sm font-medium">{step.name}</span>
              </div>
            ) : currentStep === index ? (
              <div
                className="flex w-full flex-col border-l-4 border-primary py-2 pl-4 md:border-l-0 md:border-t-4 md:pb-0 md:pl-0 md:pt-4"
                aria-current="step"
              >
                <span className="text-sm font-medium text-primary">
                  {step.id}
                </span>
                <span className="text-sm font-medium">{step.name}</span>
              </div>
            ) : (
              <div className="group flex h-full w-full flex-col border-l-4 border-gray-200 py-2 pl-4 transition-colors md:border-l-0 md:border-t-4 md:pb-0 md:pl-0 md:pt-4">
                <span className="text-sm font-medium text-gray-500 transition-colors">
                  {step.id}
                </span>
                <span className="text-sm font-medium">{step.name}</span>
              </div>
            )}
          </li>
        ))}
      </ul>
    </div> */}
    {/* <Separator /> */}
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(processForm)}
        className="space-y-8 w-full"
      >
        <div
          className={cn(
            currentStep === 1
              ? "md:inline-block w-full"
              : "md:grid md:grid-cols-3 gap-8"
          )}
        >
          {currentStep === 0 && (
            <>
              <div className="col-span-3">
                <FormField
                  control={form.control}
                  name="profileImage"
                  render={({ field }) => (
                    <FormItem>
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
                  <FormItem>
                    <FormLabel>CNIC</FormLabel>
                    <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="61101xxxxxxx"
                        {...field}
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
                  <FormItem>
                    <FormLabel>Contact Number</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        
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
                  <FormItem>
                    <FormLabel>Date of birth</FormLabel>
                    <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="Date of birth"
                        {...field}
                        type="date"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="fatherName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Father Name</FormLabel>
                    <FormControl>
                      <Input
                        disabled={loading}
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
                name="domicile"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Domicile</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="Domicile"
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
                  <FormItem>
                    <FormLabel>Province</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
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
                  <FormItem>
                    <FormLabel>Postal Code</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="Postal Code"
                        {...field}
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
                  <FormItem>
                    <FormLabel>City</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="City"
                        {...field}
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
                  <FormItem>
                    <FormLabel>Country</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="Country"
                        {...field}
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
                  <FormItem>
                    <FormLabel>Permanent Address</FormLabel>
                     <FormControl>
                      <Input
                        disabled={loading}
                        placeholder="Permanent Address"
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
                  <FormItem>
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
                  <FormItem>
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
                  <FormItem>
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
              {/* Guardian Accordion */}
              <Accordion type="single" collapsible className="col-span-3 ">
                <AccordionItem value="gInfo">
                  <AccordionTrigger
                    className={cn(
                      "[&[data-state=closed]>button]:hidden [&[data-state=open]>.alert]:hidden relative !no-underline ",
                      guardianAccordionError && "text-red-700"
                    )}
                  >
                    Guardian Information
                    {guardianAccordionError && (
                      <span className="absolute alert right-8">
                        <AlertTriangleIcon className="h-4 w-4   text-red-700" />
                      </span>
                    )}
                  </AccordionTrigger>
                  <AccordionContent>

                    <div
                      className={cn(
                        "md:grid md:grid-cols-3 gap-8 border p-4 rounded-md relative mb-4"
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
                          <FormItem  >
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
                          <FormItem >
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

              {/* Emergency Contact Accordion */}
                <Accordion type="single" collapsible className="col-span-3 ">
                <AccordionItem value="gInfo">
                  <AccordionTrigger
                    className={cn(
                      "[&[data-state=closed]>button]:hidden [&[data-state=open]>.alert]:hidden relative !no-underline ",
                      emergencyCAccordionError && "text-red-700"
                    )}
                  >
                    Emergency Contact Information
                    {emergencyCAccordionError && (
                      <span className="absolute alert right-8">
                        <AlertTriangleIcon className="h-4 w-4   text-red-700" />
                      </span>
                    )}
                  </AccordionTrigger>
                  <AccordionContent>

                    <div
                      className={cn(
                        "md:grid md:grid-cols-3 gap-8 border p-4 rounded-md relative mb-4"
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
                          <FormItem  >
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
                          <FormItem >
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
            </>
          )}
        </div>

        {/* <Button disabled={loading} className="ml-auto" type="submit">
            {action}
          </Button> */}
      </form>
    </Form>
    {/* Navigation */}
    {/* <div className="mt-8 pt-5">
      <div className="flex justify-between">
        <button
          type="button"
          onClick={prev}
          disabled={currentStep === 0}
          className="rounded bg-white px-2 py-1 text-sm font-semibold text-primary  shadow-sm ring-1 ring-inset ring-primary/30  hover:bg-green-50 disabled:cursor-not-allowed disabled:opacity-50"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth="1.5"
            stroke="currentColor"
            className="h-6 w-6"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M15.75 19.5L8.25 12l7.5-7.5"
            />
          </svg>
        </button>
        <button
          type="button"
          onClick={next}
          disabled={currentStep === steps.length - 1}
          className="rounded bg-white  px-2 py-1 text-sm font-semibold text-primary  shadow-sm ring-1 ring-inset ring-primary/30  hover:bg-green-50 disabled:cursor-not-allowed disabled:opacity-50"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth="1.5"
            stroke="currentColor"
            className="h-6 w-6"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M8.25 4.5l7.5 7.5-7.5 7.5"
            />
          </svg>
        </button>
      </div>
    </div> */}
  </>))

};
