import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  CardFooter,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { InputShowHide } from "@/components/ui/input-show-hide";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  loginFormSchema,
  loginFormSchemaType,
} from "@/lib/SchemaValidators/Authentication/Login.Validator";
import { zodResolver } from "@hookform/resolvers/zod";
import { GraduationCap, FileText } from "lucide-react";
import Link from "next/link";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

export default function Login() {
  const form = useForm<loginFormSchemaType>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });
  const {formState:{errors}} = form
  const [showPassword, setShowPassword] = useState<boolean>(false);
  //  functions
  function onSubmit(values: z.infer<typeof loginFormSchema>) {
    console.log(values);
  }

  return (
    <div className="min-h-screen bg-gray-100 py-8 px-4 sm:px-6 lg:px-8 flex items-center justify-center">
      <div className="w-full max-w-6xl flex flex-col md:flex-row gap-8">
        <Card className="flex-1">
          <CardHeader>
            <CardTitle>Important Information</CardTitle>
            <CardDescription>
              Please review the following information and required documents
              before applying for admission.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <ScrollArea className="h-[300px] rounded-md border p-4">
              <div className="space-y-4">
                <div>
                  <h4 className="text-sm font-medium flex items-center gap-2">
                    <FileText className="h-4 w-4" />
                    Documents Required for Admission:
                  </h4>
                  <ul className="mt-2 text-sm list-disc list-inside space-y-1">
                    <li>
                      Attested Copies of academic
                      (Matric/O-Level/FA/FSC/A-Level/B.Sc/BA/MA/M.SC/MBA Marks
                      sheets)
                    </li>
                    <li>Four Passport Size Photographs</li>
                    <li>Attested copy of CNIC/B.FORM</li>
                    <li>Hope Certificate for result awaited students</li>
                    <li>NTS/GAT Results for (MBA/MS/M.Phil/PhD)</li>
                  </ul>
                </div>
                <p className="text-sm text-muted-foreground">
                  Please ensure you have all the required documents ready before
                  proceeding with your application. Incomplete applications may
                  result in processing delays.
                </p>
              </div>
            </ScrollArea>
          </CardContent>
        </Card>
        <Card className="flex-1">
          <CardHeader className="space-y-1">
            <div className="flex items-center justify-center space-x-2">
              <GraduationCap className="h-6 w-6 text-primary" />
              <CardTitle className="text-2xl font-bold">
                FUUAST Admission Portal
              </CardTitle>
            </div>
            <CardDescription className="text-center">
              Welcome to the FUUAST admission portal. Please log in to continue.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <Form {...form}>
              <form onSubmit={form.handleSubmit(onSubmit)}>
                <div className="grid gap-4">
                  <FormField
                    control={form.control}
                    name="email"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Email</FormLabel>
                        <FormControl>
                          <Input placeholder="Email" error={errors?.email?.message}  {...field} />
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
                           error={errors.password?.message}
                            isPassword
                            setShowPassword={setShowPassword}
                            showPassword={showPassword}
                            placeholder="Password"
                            {...field}
                            type={!showPassword ? "password" : "text"}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <Button className="w-full">Log in</Button>
                </div>
              </form>
            </Form>
          </CardContent>
          <CardFooter className="flex flex-col space-y-2 text-sm text-muted-foreground">
            <div className="flex justify-between w-full">
              <Link href="/sign-up" className="hover:underline">
                Create an account
              </Link>
              <Link href="/forgot-password" className="hover:underline">
                Forgot password?
              </Link>
            </div>
            <p className="text-center">
              By logging in, you agree to our{" "}
              <Link href="/terms" className="hover:underline">
                Terms of Service
              </Link>{" "}
              and{" "}
              <Link href="/privacy" className="hover:underline">
                Privacy Policy
              </Link>
              .
            </p>
          </CardFooter>
        </Card>
      </div>
    </div>
  );
}

// import { Button } from "@/components/ui/button";
// import {
//   Form,
//   FormControl,
//   FormField,
//   FormItem,
//   FormLabel,
//   FormMessage,
// } from "@/components/ui/form";
// import { Input } from "@/components/ui/input";
// import { InputShowHide } from "@/components/ui/input-show-hide";
// import {
//   loginFormSchema,
//   loginFormSchemaType,
// } from "@/lib/SchemaValidators/Authentication/Login.Validator";
// import { AppLogo } from "@/utils/icons/AppIcons";
// import { AuthRoutes } from "@/utils/routes/Routes";
// import { zodResolver } from "@hookform/resolvers/zod";
// import Image from "next/image";
// import Link from "next/link";
// import React, { FC, useState } from "react";
// import { useForm } from "react-hook-form";
// import { z } from "zod";

// type LoginProps = {};

// const Login: FC<LoginProps> = ({}) => {
//   //  hooks
//   const form = useForm<loginFormSchemaType>({
//     resolver: zodResolver(loginFormSchema),
//     defaultValues: {
//       email: "",
//       password: "",
//     },
//   });
//   const [showPassword, setShowPassword] = useState<boolean>(false);
//   //  functions
//   function onSubmit(values: z.infer<typeof loginFormSchema>) {
//     console.log(values);
//   }
//   return (
//     <main className="w-full h-screen flex bg-[url(/UniPhotos/photo9.jpg)]  bg-cover bg-no-repeat bg-center">
//    <div className="w-full flex justify-center items-center ">
//      {/* <div className="grid"> */}
//         <div className="w-full max-w-sm border backdrop-blur-xl bg-white/30    first-line:space-y-3 px-5 py-6 rounded-xl ">
//           <div className="text-center pb-4">
//             <div className="flex items-center justify-center">
//               <Image src={AppLogo} alt="logo" height={150} />
//             </div>
//             <div className="mt-4">
//               <h3 className="text-gray-800 text-2xl font-bold sm:text-3xl">
//                 Log in to your account
//               </h3>
//             </div>
//             <p className="mt-3">
//               Don&apos;t have an account?{" "}
//               <Link
//                 href={AuthRoutes.SignUp}
//                 className="font-medium  text-[hsl(var(--primary))] "
//               >
//                 Signup
//               </Link>
//             </p>
//           </div>
//           <Form    {...form}>
//             <form onSubmit={form.handleSubmit(onSubmit)} >
//               <FormField
//                 control={form.control}
//                 name="email"
//                 render={({ field }) => (
//                   <FormItem>
//                     <FormLabel>Email</FormLabel>
//                     <FormControl>
//                       <Input placeholder="Email" {...field} />
//                     </FormControl>
//                     <FormMessage />
//                   </FormItem>
//                 )}
//               />
//               <FormField
//                 control={form.control}
//                 name="password"
//                 render={({ field }) => (
//                   <FormItem>
//                     <FormLabel>Password</FormLabel>
//                     <FormControl>
//                       <InputShowHide
//                         isPassword
//                         setShowPassword={setShowPassword}
//                         showPassword={showPassword}
//                         placeholder="Password"
//                         {...field}
//                         type={!showPassword ? "password" : "text"}
//                       />
//                     </FormControl>
//                     <FormMessage />
//                   </FormItem>
//                 )}
//               />
//               <Button className="text-center w-full mt-4" type="submit">
//                 Login
//               </Button>
//             </form>
//           </Form>
//         </div>
//       {/* </div> */}
//      </div>
//     </main>
//   );
// };

// export default Login;
