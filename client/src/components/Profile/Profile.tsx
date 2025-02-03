"use client"
import { Button } from "@/components/ui/button"
import { Heading } from "@/components/ui/heading"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { zodResolver } from "@hookform/resolvers/zod"
import { FC, useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { z } from "zod"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../ui/form"
import { InputShowHide } from "../ui/input-show-hide"
import { useChangePasswordMutation, useLazyLogoutQuery } from "@/redux/features/auth/userApi"
import { toast } from "../ui/use-toast"
import { CheckCircle2, Loader } from "lucide-react"
import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks"
import { RootState } from "@/redux/store"
import { removeRefreshTokenDateTime } from "@/lib/services/authLocalStorageService"
import { redirect, useRouter } from "next/navigation"
import { userLoggedOut } from "@/redux/features/auth/userSlice"
import { clearCurrentStepId } from "@/lib/services/wizardLocalStorageService"
import { Avatar, AvatarFallback, AvatarImage } from "../ui/avatar"

type Props = {}
const getCharacterValidationError = (str: string) => {
    return `Your password must have at least 1 ${str} character`;
};
const changePasswordSchema = z.object({
  password: z.string().min(8, "Your Password must contain minimum 8 character(s)"),
  newPassword: z.string()
        .regex(/[0-9]/, getCharacterValidationError("digit"))
        .regex(/[a-z]/, getCharacterValidationError("lowercase"))
        .regex(/[A-Z]/, getCharacterValidationError("uppercase")),
    confirmNewPassword: z.string().min(8, "Confirm Password must contain at least 8 character(s)")
}).refine((data) => data.newPassword === data.confirmNewPassword, {
    message: "Passwords don't match",
    path: ["confirmNewPassword"],
});
type ChangePassword =z.infer< typeof changePasswordSchema>
const title = "Profile settings"
const description  = "User information and password settings"
const Profile : FC<Props> = ({}) => { 
    const form = useForm<ChangePassword>({
      resolver: zodResolver(changePasswordSchema),
      defaultValues: {
        newPassword: "",
        password: "",
        confirmNewPassword: ''
      },
    });
    const user = useAppSelector((state : RootState) => state.auth.user) 
    const [changePassword, { isLoading , data}] = useChangePasswordMutation()
    /**
     * This function will be called when the form is submitted.
     * 
     * @param {ChangePassword} data The data that will be submitted.
     * This data will contain the new password, confirm password and the old password.
     * The new password and confirm password must match.
     * The old password is needed to verify the user before updating the password.
     */
    const onSubmit = async (data: ChangePassword) => {
     await changePassword(data).unwrap().then((data) => {
       toast({
         title: "Password Updated",
         description: data || "Your password has been updated successfully",
         action: (
           <div className="flex items-center">
             <CheckCircle2 className="mr-2 h-5 w-5 text-green-500" />
             <span className="font-semibold text-green-500">Success</span>
           </div>
         ),
         className: "bg-white dark:bg-gray-800 border-green-500 border-2",
         duration: 5000,
       })
      
       setTimeout(() => {
        dispatch(userLoggedOut());
        removeRefreshTokenDateTime();
        clearCurrentStepId();
        redirect("/login");
       }, 5000);
     });
    }
    const {formState:{errors}} = form
    const [showPassword, setShowPassword] = useState<boolean>(false);
    const [showNewPassword, setShowNewPassword] = useState<boolean>(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState<boolean>(false);

      const [logoutTrigger,{isSuccess}] = useLazyLogoutQuery();
  const dispatch = useAppDispatch()

  return (
<>
<Heading description={description} title={title}/>
<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
           <div className="">
              <div className="flex justify-center">
              <Avatar className="size-20 md:size-32">
            <AvatarImage src={user?.profilePictureUrl} alt={""} />
            <AvatarFallback>{user?.fullName?.charAt(0)+"."}</AvatarFallback>
          </Avatar>
              </div>
            <div className="mb-4">
            <Label className="mb-2">
                Username
            </Label>
             <Input value={user?.userName} disabled  />
           </div>
          <div className="mb-4">
            <Label className="mb-2">
                Full Name
            </Label>
             <Input value={user?.fullName} disabled  />
           </div>
           <div className="mb-4">
            <Label className="mb-2">
                Email
            </Label>
             <Input value={user?.email} disabled type="email"  />
           </div>
           </div>
         <Form {...form}>
              <form onSubmit={form.handleSubmit(onSubmit)}>
                <div className="grid gap-4 md:mt-[123px]">
                  <FormField
                    control={form.control}
                    name="password"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel> Password</FormLabel>
                        <FormControl>
                        <InputShowHide
                           error={errors.password?.message}
                            isPassword
                            setShowPassword={setShowPassword}
                            showPassword={showPassword}
                            placeholder="Current Password"
                            {...field}
                            type={!showPassword ? "password" : "text"}
                          />                        
                          </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="newPassword"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>New Password</FormLabel>
                        <FormControl>
                          <InputShowHide
                           error={errors.newPassword?.message}
                            isPassword
                            setShowPassword={setShowNewPassword}
                            showPassword={showNewPassword}
                            placeholder="New Password"
                            {...field}
                            type={!showNewPassword ? "password" : "text"}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                      
                    )}
                  />
                         <FormField
                    control={form.control}
                    name="confirmNewPassword"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Confirm Password</FormLabel>
                        <FormControl>
                          <InputShowHide
                           error={errors.confirmNewPassword?.message}
                            isPassword
                            setShowPassword={setShowConfirmPassword}
                            showPassword={showConfirmPassword}
                            placeholder="Password"
                            {...field}
                            type={!showConfirmPassword ? "password" : "text"}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
<Button className="w-full" disabled={isLoading}>
  {isLoading ? (
    <Loader className="w-4 h-4 animate-spin text-primary" />
  ) : (
    "Change Password"
  )}
</Button>                </div>
              </form>
            </Form>
         
    </div> 
</>
    
  )
}

export default Profile