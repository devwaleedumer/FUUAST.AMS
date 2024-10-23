"use client";
import { apiSlice } from "@/redux/features/apiSlice";
import LayoutLoader from "./shared/LayoutLoader";
import { redirect, useParams, usePathname } from "next/navigation";
import { useEffect } from "react";
import { useAppSelector } from "@/hooks/reduxHooks";
import { RootState } from "@/redux/store";
import { useRouter } from "next/navigation";
import { AuthRoutes } from "@/utils/routes/Routes";

const ROUTE_ROLES = [
  /**
   * For authentication pages
   * @example /login /register
   */
  'auth',
  /**
   * Optional authentication
   * It doesn't push to login page if user is not authenticated
   */
  'optional',
  /**
   * For all authenticated user
   * will push to login if user is not authenticated
   */
  'all',
] as const;
type RouteRole = (typeof ROUTE_ROLES)[number];

export default function isAuth(Component: any,routeRole: RouteRole) {
  return function IsAuth(props: any) {
  const router = useRouter();
  const {redirect} = useParams()
  const currentPath = usePathname()
  const isAuthenticated = useAppSelector((state: RootState) => state.auth.isAuthenticated)
  const { isLoading, isFetching,data:user,error } = apiSlice.endpoints.loadUser.useQuery(null, {
    skip: isAuthenticated || routeRole == "auth",
    refetchOnMountOrArgChange: true,
  });
  const loading = isLoading || isFetching;
 useEffect(() => {
      // If loading, prevent any redirection until data is fetched
      if (loading) return;

      if (!isAuthenticated || error) {
        if (routeRole === "optional") {
          router.replace(AuthRoutes.Login)
          return 
        }
          if(routeRole === "auth") 
              return
        // }
      } else if (isAuthenticated) {
        if (routeRole === "auth") {
          if (redirect) {
            router.replace(redirect as string);
          } else {
            router.replace("/dashboard");
          }
        }
      }
    }, [user, loading, isAuthenticated]);

  
  return isLoading ? <LayoutLoader/> : <Component {...props} />  ;
  };
}