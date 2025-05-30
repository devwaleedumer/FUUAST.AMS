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
import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks";
import {
  bloodGroups,
  genders,
  religions,
  photographRequirements,
  pakistanCitiesByProvince,
  relations,
  countrie,
} from "@/lib/data";
import {
  PersonalEditInfoValues,
  personalInfo,
  personalInfoDefaults,
  personalInfoEditSchema,
} from "@/lib/SchemaValidators/ApplicationForm/PersonalInfoSchema.Validator";
import { cn, omitProps } from "@/lib/utils";
import {
  useEditApplicantPersonalInformationMutation,
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
  LoaderCircle,
  SaveAll,
  ChevronsUpDown,
  Check,
} from "lucide-react";
import { FC, useEffect, useState } from "react";
import { FieldErrors, SubmitHandler, useForm } from "react-hook-form";
import { AspectRatio } from "../../ui/aspect-ratio";
import { FaTrashAlt } from "react-icons/fa";
import { PersonalInformation } from "@/types/applicant";
import { toast } from "../../ui/use-toast";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
const description =
  "For processing your application, we first following information about you.";
type EditPersonalInfoProps = {
  personalInformationData: PersonalInformation | undefined;
};

const EditPersonalInfo: FC<EditPersonalInfoProps> = ({
  personalInformationData,
}) => {
  type PersonalEditInfoValuesTrue = Extract<
    PersonalEditInfoValues,
    { isImageChanged: true }
  >;
  const form = useForm<PersonalEditInfoValues>({
    resolver: zodResolver(personalInfoEditSchema),
    defaultValues: { ...personalInfoDefaults, isImageChanged: false },
    mode: "onTouched",
    values: personalInformationData as any,
  });

  const [edit, { isLoading, isSuccess }] =
    useEditApplicantPersonalInformationMutation();
  const [isOpen, setIsOpen] = useState(false);
  const [isCountryListOpen, setIsCountryListOpen] = useState(false);
  useEffect(() => {
    if (personalInformationData!.dob) {
      setValue("dob", new Date(personalInformationData!.dob));
    }
    if ((personalInformationData as any).profilePictureUrl) {
      setValue("isImageChanged", false);
    } else {
      setValue("isImageChanged", true);
    }
    // setValue("isImageChanged",false)
    if (personalInformationData?.domicile) {
      setCityList(
        pakistanCitiesByProvince.filter(
          (x) => x.province == personalInformationData?.province
        )[0].cities
      );
      setValue("domicile", personalInformationData?.domicile);
      setValue("province", personalInformationData?.province);
    }
  }, []);
  useEffect(() => {
    if (isSuccess) {
      toast({
        title: "Success",
        description: "Personal data successfully updated",
        variant: "default",
      });
      dispatch(nextStep());
    }
  }, [isSuccess]);
  const [cityList, setCityList] = useState<Array<string>>([]);
  const onChangeProvince = (province: string) => {
    if (province) {
      setCityList(
        pakistanCitiesByProvince.filter((x) => x.province == province)[0]
          .cities ?? []
      );
    }
    setValue("domicile", "");
  };
  const [loading, setLoading] = useState(false);
  const dispatch = useAppDispatch();
  const {
    setValue,
    getValues,
    watch,
    formState: { errors },
  } = form;
  const isImageChanged = watch("isImageChanged");
  const processPersonalInfoForm: SubmitHandler<PersonalEditInfoValues> = async (
    data
  ) => {
    // Dispatch and next step
    await edit(data);
  };
  return (
    <>
      <Heading
        title={"Step#1. Personal Information"}
        description={description}
      />
      <Card>
        <CardContent>
          {/*<List className="p-5" list={photographRequirements} title="Physical Attributes of Photograph"/>*/}
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(processPersonalInfoForm, (error) => {
                console.log(error);
              })}
              className="space-y-4 w-full pt-5"
            >
              <div className={cn("md:grid md:grid-cols-3 gap-x-3 ")}>
                <>
                  {/* Create  */}

                  {/* // Image remain unchanged */}
                  {isImageChanged == false ? (
                    <div className=" col-span-3">
                      <FormLabel>Photo</FormLabel>
                      <div className=" flex justify-center items-center">
                        <div
                          className={`border relative md:mb-4  size-24 md:size-32  rounded-full overflow-hidden ring-2 ring-primary hover:opacity-70 transition-opacity duration-100 ease-in-out`}
                        >
                          <AspectRatio ratio={1 / 1} className="relative">
                            <img
                              src={getValues<any>("profilePictureUrl")}
                              className="w-full h-full"
                              alt="upload item"
                            />
                          </AspectRatio>
                          <FaTrashAlt
                            className="h-4 w-5 z-30 absolute text-green-700 hover:text-green-500 top-[17%] left-[68%] cursor-pointer"
                            onClick={() => {
                              setValue("isImageChanged", true);
                            }}
                          />
                        </div>
                      </div>
                    </div>
                  ) : (
                    // Image got changed
                    <div className="col-span-3">
                      <FormField
                        control={form.control}
                        name="profileImage"
                        render={({ field }) => (
                          <FormItem className="mb-2">
                            <FormLabel className="relative">
                              Photo{" "}
                              <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                            </FormLabel>
                            <FormControl>
                              <ProfileFileUpload
                                setValue={setValue as any}
                                onChange={field.onChange}
                                value={field.value}
                                isValid={
                                  !!(
                                    errors as FieldErrors<PersonalEditInfoValuesTrue>
                                  ).profileImage
                                }
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    </div>
                  )}

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
                            disabled
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
                        <Popover open={isOpen} onOpenChange={setIsOpen}>
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
                              captionLayout="dropdown"
                              fromYear={2015}
                              toYear={2025}
                              selected={
                                (field.value as any) !== typeof Date
                                  ? new Date(field.value)
                                  : field.value
                              }
                              onSelect={field.onChange}
                              onDayClick={() => setIsOpen(false)}
                              defaultMonth={field.value}
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
                          value={field.value || personalInformationData?.gender}
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
                          value={
                            field.value || personalInformationData?.bloodGroup
                          }
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
                          value={
                            field.value || personalInformationData?.religion
                          }
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
                    name="province"
                    render={({ field }) => (
                      <FormItem className="mb-2">
                        <FormLabel className="relative">
                          Province{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>
                        <Select
                          disabled={loading}
                          onValueChange={(e) => {
                            onChangeProvince(e as string);
                            field.onChange(e);
                          }}
                          value={field.value || personalInformationData?.province || ''}
                          defaultValue={field.value}
                        >
                          <FormControl>
                            <SelectTrigger error={errors.province?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a Province"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {pakistanCitiesByProvince.map((p_c) => (
                              <SelectItem
                                key={p_c.province + "pvc"}
                                value={p_c.province}
                              >
                                {p_c.province}
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
                        <Select
                          disabled={loading}
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                          value={field.value || personalInformationData?.domicile || ''}
                        >
                          <FormControl>
                            <SelectTrigger error={errors.domicile?.message}>
                              <SelectValue
                                defaultValue={field.value}
                                placeholder="Select a Domicile"
                              />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {/* @ts-ignore  */}
                            {cityList.map((p_c) => (
                              <SelectItem key={p_c + "city"} value={p_c}>
                                {p_c}
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
                    name="country"
                    render={({ field }) => (
                      <FormItem className="flex flex-col space-y-2 mt-2">
                        <FormLabel className="relative">
                          Country{" "}
                          <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                        </FormLabel>{" "}
                        <Popover
                          open={isCountryListOpen}
                          onOpenChange={setIsCountryListOpen}
                        >
                          <PopoverTrigger asChild>
                            <FormControl>
                              <Button
                                variant="outline"
                                role="combobox"
                                className={cn(
                                  "justify-between",
                                  !field.value && "text-muted-foreground"
                                )}
                              >
                                {field.value
                                  ? countrie.find(
                                      (country) => country === field.value
                                    )
                                  : "Select Country"}
                                <ChevronsUpDown className="opacity-50" />
                              </Button>
                            </FormControl>
                          </PopoverTrigger>
                          <PopoverContent className="w-full p-0">
                            <Command>
                              <CommandInput
                                placeholder="Search country."
                                className="h-9"
                              />
                              <CommandList>
                                <CommandEmpty>No Country found.</CommandEmpty>
                                <CommandGroup>
                                  {countrie.map((c) => (
                                    <CommandItem
                                      value={c}
                                      key={c}
                                      onSelect={() => {
                                        form.setValue("country", c);
                                        setIsCountryListOpen(false);
                                      }}
                                    >
                                      {c}
                                      <Check
                                        className={cn(
                                          "ml-auto",
                                          c === field.value
                                            ? "opacity-100"
                                            : "opacity-0"
                                        )}
                                      />
                                    </CommandItem>
                                  ))}
                                </CommandGroup>
                              </CommandList>
                            </Command>
                          </PopoverContent>
                        </Popover>
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
                                      placeholder="Guardian's Name"
                                    />
                                  </FormControl>
                                  <FormMessage />
                                </FormItem>
                              )}
                            />
                            <FormField
                              control={form.control}
                              name="guardian.relation"
                              render={({ field }) => (
                                <FormItem className="mb-2">
                                  <FormLabel className="relative">
                                    Guardian Relation{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <Select
                                    disabled={loading}
                                    onValueChange={field.onChange}
                                    value={
                                      field.value ||
                                      personalInformationData?.guardian
                                        ?.relation || ""
                                    }
                                    defaultValue={field.value}
                                  >
                                    <FormControl>
                                      <SelectTrigger
                                        error={
                                          errors.guardian?.relation?.message
                                        }
                                      >
                                        <SelectValue
                                          defaultValue={field.value}
                                          placeholder="Select a Relation"
                                        />
                                      </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                      {/* @ts-ignore  */}
                                      {relations.map((rlgn) => (
                                        <SelectItem
                                          key={rlgn + "rel"}
                                          value={rlgn}
                                        >
                                          {rlgn}
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
                              name="emergencyContact.relation"
                              render={({ field }) => (
                                <FormItem className="mb-2">
                                  <FormLabel className="relative">
                                    Contact's Relation{" "}
                                    <Asterisk className="size-2 inline-flex absolute top-[2px]" />{" "}
                                  </FormLabel>
                                  <Select
                                    disabled={loading}
                                    onValueChange={field.onChange}
                                    value={
                                      field.value ||
                                      personalInformationData?.emergencyContact
                                        ?.relation ||
                                      ""
                                    }
                                    defaultValue={field.value}
                                  >
                                    <FormControl>
                                      <SelectTrigger
                                        error={
                                          errors.emergencyContact?.relation
                                            ?.message
                                        }
                                      >
                                        <SelectValue
                                          defaultValue={field.value}
                                          placeholder="Select a Relation"
                                        />
                                      </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                      {/* @ts-ignore  */}
                                      {relations.map((rlgn) => (
                                        <SelectItem
                                          key={rlgn + "em-rel"}
                                          value={rlgn}
                                        >
                                          {rlgn}
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
              <div className=" flex justify-between w-full">
                <Button
                  type="button"
                  disabled={true}
                  onClick={(e) => dispatch(prevStep())}
                  size={"sm"}
                >
                  <ArrowLeft className="size-4 " />
                  Previous
                </Button>

                {personalInformationData?.emergencyContact == null ? (
                  <Button disabled={isLoading} type="submit" size={"sm"}>
                    {!isLoading ? (
                      <>
                        {" "}
                        Save & Next
                        <Save className="size-4 " />
                      </>
                    ) : (
                      <LoaderCircle className="size-4 animate-spin" />
                    )}
                  </Button>
                ) : (
                  <div className="space-x-2 flex">
                    <Button disabled={isLoading} type="submit" size={"sm"}>
                      {!isLoading ? (
                        <>
                          Update
                          <Save className="size-4 " />
                        </>
                      ) : (
                        <LoaderCircle className="size-4 animate-spin" />
                      )}
                    </Button>
                    <Button
                      type="button"
                      disabled={isLoading}
                      size={"sm"}
                      onClick={() => dispatch(nextStep())}
                    >
                      Next
                      <ArrowRight className="size-4 " />
                    </Button>
                  </div>
                )}
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </>
  );
};

export default EditPersonalInfo;
