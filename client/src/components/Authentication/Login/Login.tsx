import { Button } from "@/components/ui/button";
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
import {
  loginFormSchema,
  loginFormSchemaType,
} from "@/lib/SchemaValidators/Authentication/Login.Validator";
import { AppLogo } from "@/utils/icons/AppIcons";
import { AuthRoutes } from "@/utils/routes/Routes";
import { zodResolver } from "@hookform/resolvers/zod";
import Image from "next/image";
import Link from "next/link";
import React, { FC, useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

type LoginProps = {};

const Login: FC<LoginProps> = ({}) => {
  //  hooks
  const form = useForm<loginFormSchemaType>({
    resolver: zodResolver(loginFormSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  //  functions
  function onSubmit(values: z.infer<typeof loginFormSchema>) {
    console.log(values);
  }
  return (
    <main className="w-full h-screen flex">
      <div className="w-full flex justify-center items-center">
        <div className="w-full max-w-sm first-line:space-y-3 px-5">
          <div className="text-center pb-4">
            <div className="flex items-center justify-center">
              <Image src={AppLogo} alt="logo" height={200} />
            </div>
            <div className="mt-4">
              <h3 className="text-gray-800 text-2xl font-bold sm:text-3xl">
                Log in to your account
              </h3>
            </div>
            <p className="mt-3">
              Don&apos;t have an account?{" "}
              <Link
                href={AuthRoutes.SignUp}
                className="font-medium  text-[hsl(var(--primary))] "
              >
                Signup
              </Link>
            </p>
          </div>
          <Form {...form}>
            <form>
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
              <Button className="text-center w-full mt-4" type="submit">
                Login
              </Button>
            </form>
          </Form>
        </div>
      </div>
    </main>
  );
};

export default Login;
