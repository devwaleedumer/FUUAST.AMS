"use client";
import PageLoader from "@/components/shared/Loader";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks";
import { removeRefreshTokenDateTime } from "@/lib/services/authLocalStorageService";
import { clearCurrentStepId } from "@/lib/services/wizardLocalStorageService";
import { useLazyLogoutQuery } from "@/redux/features/auth/userApi";
import { userLoggedOut } from "@/redux/features/auth/userSlice";
import { RootState } from "@/redux/store";
import Link from "next/link";
import { redirect, useRouter } from "next/navigation";
import { useEffect } from "react";
export function Navbar() {
  const {user} = useAppSelector((state : RootState) => state.auth)
  const [logoutTrigger,{isSuccess}] = useLazyLogoutQuery();
  const dispatch = useAppDispatch()
  useEffect(() => {
    if (isSuccess) {
      dispatch(userLoggedOut());
      removeRefreshTokenDateTime();
      clearCurrentStepId();
      redirect("/login");
    }
  }, [isSuccess])
  
  const handleLogoutTrigger = async () => {
    await logoutTrigger(null);
  }
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="relative h-8 w-8 rounded-full">
          <Avatar className="h-8 w-8">
            <AvatarImage src={user?.profilePictureUrl} alt={""} />
            <AvatarFallback>{user?.fullName?.charAt(0)+"."}</AvatarFallback>
          </Avatar>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-56" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <p className="text-sm font-medium leading-none">{user?.fullName}</p>
            <p className="text-xs leading-none text-muted-foreground">
              {user?.email}
            </p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          <DropdownMenuItem>
           <Link href={"/profile"}>Profile</Link>
          </DropdownMenuItem>
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={handleLogoutTrigger}>
          Log out
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
