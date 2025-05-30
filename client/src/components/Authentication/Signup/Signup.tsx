import React, { FC, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import {
  signUpFormSchema,
  SignupValues,
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
import Link from "next/link";
import { AuthRoutes } from "@/utils/routes/Routes";
import { InputShowHide } from "@/components/ui/input-show-hide";
import isAuth from "@/components/ApplicantProtected";
import { BookOpen, CheckCircle2, LoaderCircle } from "lucide-react";
import { useRegisterMutation } from "@/redux/features/auth/userApi";
import { toast } from "@/components/ui/use-toast";
import { redirect } from "next/navigation";

type SignupProps = {};

const Signup: FC<SignupProps> = ({}) => {
  //  hooks
  const form = useForm<SignupValues>({
    resolver: zodResolver(signUpFormSchema),
    defaultValues: {
      fullName: "",
      email: "",
      password: "",
      confirmPassword: "",
      cnic:""
    },
    mode: "onChange",
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [showCPassword, setShowCPassword] = useState<boolean>(false);
  //  functions
  const [register, {isLoading,data,isSuccess}] = useRegisterMutation();
 async  function onSubmit(values: z.infer<typeof signUpFormSchema>) {
    await register(values)
  }
   useEffect(() => {
    if(isSuccess){
       toast({
        title:"Success",
        description: data,
        className: "bg-background border-l-8 border-l-green-500",
        action: (
                <div className="flex items-center">
                  <CheckCircle2 className="mr-2 h-5 w-5 text-green-500" />
                  <span className="font-semibold text-green-500">Success</span>
                </div>
              ),
        // i
      })
      redirect("/login")
    }
    
  },[isSuccess])
  return (
    <main className="w-full min-h-screen flex px-4 py-6 lg:py-8 bg-gradient-to-br from-green-400 via-emerald-500 to-teal-600">
      <div className="flex w-full items-center justify-center ">
        <div className=" w-full  max-w-md first-line:space-y-3  rounded-lg p-6 bg-white/95">
            <div className="mt-1 text-center pb-10">
              <div className="flex items-center justify-center mb-2">
            <BookOpen className="h-12 w-12 text-emerald-600" />
          </div>
              <h3 className="text-gray-800 text-2xl font-bold sm:text-3xl">
                {/* Sign up to your account */}
                Start your journey at FUUAST
              </h3>
              <p className="text-sm">
                 Begin your academic journey with us
              </p>
              {/* <div className="flex items-center justify-center">
                    <Image src={AppLogo} alt="logo" height={100} />
                  </div> */}
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
                  name="cnic"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>
                        CNIC
                      </FormLabel>
                      <FormControl>
                        <Input placeholder="6110112346789"  {...field} />
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
              <Button 
              type="submit"
              disabled={isLoading} 
              className="w-full mt-4 bg-gradient-to-r from-emerald-500 to-green-600 hover:from-emerald-600 hover:to-green-700 text-white font-semibold py-2 px-4 rounded-md transition duration-300 ease-in-out transform hover:scale-105"
            >
            {isLoading ? <LoaderCircle className="size-4 animate-spin"/>:   ("Register")}
            </Button>
              <p className="pt-5">
                  Have already an account?{" "}
                <Link
                  href={AuthRoutes.Login}
                  className="font-medium text-[hsl(var(--primary))] "
                >
                  Login
                </Link>
              </p>
            </form>
          </Form>
        </div>
      </div>
    </main>
  );
};

export default Signup;
