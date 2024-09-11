import React, { FC, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import Image from "next/image";
import {
  signUpFormSchema,
  signUpFormSchemaType,
} from "@/lib/SchemaValidators/Authentication/Signup.Validator";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { AppLogo, UniversityPhoto2 } from "@/utils/icons/AppIcons";
import Link from "next/link";
import { AuthRoutes } from "@/utils/routes/Routes";
import { InputShowHide } from "@/components/ui/input-show-hide";

type SignupProps = {};

const Signup: FC<SignupProps> = ({}) => {
  //  hooks
  const form = useForm<signUpFormSchemaType>({
    resolver: zodResolver(signUpFormSchema),
    defaultValues: {
      fullName: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [showCPassword, setShowCPassword] = useState<boolean>(false);
  //  functions
  function onSubmit(values: z.infer<typeof signUpFormSchema>) {
    console.log(values);
  }

  return (
    <main className="w-full min-h-screen flex px-4 py-6 lg:py-8 ">
      {/* <div className="w-1/2    h-full">
        <Image
          src={UniversityPhoto2}
          alt="University Garden"
          className="h-full w-full"
        />
      </div> */}
      <div className="flex w-full items-center justify-center">
        <div className="w-full max-w-md first-line:space-y-3 border rounded-lg p-6 ">
          <div className="text-center pb-4">
            <div className="mt-1">
              <h3 className="text-gray-800 text-2xl font-bold sm:text-3xl">
                Sign up to your account
              </h3>
              <p className="">
                Have already an account?{" "}
                <Link
                  href={AuthRoutes.Login}
                  className="font-medium text-[hsl(var(--primary))] "
                >
                  Login
                </Link>
              </p>
              {/* <div className="flex items-center justify-center">
                    <Image src={AppLogo} alt="logo" height={100} />
                  </div> */}
            </div>
          </div>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
              <div className="space-y-5">
                <FormField
                  control={form.control}
                  name="fullName"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>
                        Full Name
                        <FormDescription className="ml-1 inline">
                          (as per CNIC)
                        </FormDescription>
                      </FormLabel>
                      <FormControl>
                        <Input placeholder="Full Name" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email</FormLabel>
                      <FormControl>
                        <Input placeholder="Email" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="password"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Password</FormLabel>
                      <FormControl>
                        <InputShowHide
                          placeholder="Password"
                          type={!showPassword ? "password" : "text"}
                          {...field}
                          isPassword
                          setShowPassword={setShowPassword}
                          showPassword={showPassword}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="confirmPassword"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Confirm Password</FormLabel>
                      <FormControl>
                        <InputShowHide
                          type={!showCPassword ? "password" : "text"}
                          isPassword
                          setShowPassword={setShowCPassword}
                          showPassword={showCPassword}
                          placeholder="Confirm Password"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <Button className="text-center w-full mt-4" type="submit">
                Register
              </Button>
            </form>
          </Form>
        </div>
      </div>
    </main>
  );
};

export default Signup;
