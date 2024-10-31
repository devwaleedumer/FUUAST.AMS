'use client';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu';
import { useAppDispatch, useAppSelector } from '@/hooks/reduxHooks';
import { removeRefreshTokenDateTime } from '@/lib/services/authLocalStorageService';
import { clearCurrentStepId } from '@/lib/services/wizardLocalStorageService';
import { useLazyLogoutQuery } from '@/redux/features/auth/userApi';
import { userLoggedOut } from '@/redux/features/auth/userSlice';
import { RootState } from '@/redux/store';
import { redirect } from 'next/navigation';
import { useEffect } from 'react';
export function UserNav() {
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
  
  const signOut = async () => {
    await logoutTrigger(null);
  }
  if (user) {
    return (
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" className="relative h-8 w-8 rounded-full">
            <Avatar className="h-8 w-8">
              <AvatarImage
                src={user?.profilePictureUrl ?? ''}
                alt={user?.fullName ?? ''}
              />
              <AvatarFallback>{user?.fullName?.[0]}</AvatarFallback>
            </Avatar>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent className="w-56" align="end" forceMount>
          <DropdownMenuLabel className="font-normal">
            <div className="flex flex-col space-y-1">
              <p className="text-sm font-medium leading-none">
                {user?.fullName}
              </p>
              <p className="text-xs leading-none text-muted-foreground">
                {user?.fullName}
              </p>
            </div>
          </DropdownMenuLabel>
          <DropdownMenuSeparator />
          <DropdownMenuGroup>
            <DropdownMenuItem>
              Change Password
              <DropdownMenuShortcut>⇧⌘P</DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuItem>
              Settings
              <DropdownMenuShortcut>⌘S</DropdownMenuShortcut>
            </DropdownMenuItem>
          </DropdownMenuGroup>
          <DropdownMenuSeparator />
          <DropdownMenuItem onClick={async() => await signOut()}>
            Log out
            <DropdownMenuShortcut>⇧⌘Q</DropdownMenuShortcut>
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
    );
  }
}