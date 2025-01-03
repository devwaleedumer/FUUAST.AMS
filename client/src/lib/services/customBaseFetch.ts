import { AuthRoutes } from '@/utils/routes/Routes'
import { fetchBaseQuery } from '@reduxjs/toolkit/query'
import type {
    BaseQueryFn,
    FetchArgs,
    FetchBaseQueryError,
} from '@reduxjs/toolkit/query'
import { Mutex } from 'async-mutex'
import { getRefreshTokenDateTime, removeRefreshTokenDateTime, setRefreshTokenDateTime } from './authLocalStorageService'
import { ILoginResponse } from '@/types/auth'
import { toast } from '@/components/ui/use-toast'
import { useDispatch } from 'react-redux'
import { initializeState, resetState } from '@/redux/features/applicant/applicationWizardSlice'
import { setCurrentStepId } from './wizardLocalStorageService'

// create a new mutex
const mutex = new Mutex()
const baseQuery = fetchBaseQuery({
    baseUrl: process.env.NEXT_PUBLIC_SERVER_URI,
    credentials: "include" as const
})
const refreshOrFailRequestUsingMutex: BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> = async (args, api, extraOptions) => {
    // wait until the mutex is available without locking it
    await mutex.waitForUnlock()
    let result = await baseQuery(args, api, extraOptions)
    if (result.error?.status === "FETCH_ERROR" && result.error.error === "TypeError: NetworkError when attempting to fetch resource.") {
        toast({ title: "Network Error!", description: "Network has went away check your connectivity", variant: "destructive" })
        removeRefreshTokenDateTime();
    }
    console.log(result.error?.data)
    if (result.error && result.error.status === 401
        && (result.error?.data as any).detail == "Authentication Failed.") {
        // checking whether the mutex is locked
        if (!mutex.isLocked()) {

            const release = await mutex.acquire()
            try {
                const refreshResult = await baseQuery(
                    '/tokens/refresh-cookie',
                    api,
                    extraOptions
                )
                if (refreshResult.meta?.response?.status === 200) {
                    // retry the initial query
                    setRefreshTokenDateTime((refreshResult?.data as ILoginResponse).refreshTokenExpiryTime)
                    result = await baseQuery(args, api, extraOptions)

                } else {
                    removeRefreshTokenDateTime()
                    window.location.href = AuthRoutes.Login
                }
            } finally {
                // release must be called once the mutex should be released again.
                release()
            }
        } else {
            // wait until the mutex is available without locking it
            await mutex.waitForUnlock()
            result = await baseQuery(args, api, extraOptions)
        }
    }
    return result
}
const authRoutes = [
    "register", "login", "verifyEmail"
]
const gracedEndPoints = ["loadUser"]
const customFetchBaseQuery: BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> = async (args, api, extraOptions) => {
    const refreshTokenExpiryTime = getRefreshTokenDateTime()
    const isAuthRoute = authRoutes.find(x => x == api.endpoint);
    const refreshDtExpiredOrNull = refreshTokenExpiryTime == null || new Date() > new Date(refreshTokenExpiryTime as string);
    if (isAuthRoute) {
        return await baseQuery(args, api, extraOptions)
    }
    // if (gracedEndPoints.find(x => x == api.endpoint))
    //     return await refreshOrFailRequestUsingMutex(args, api, extraOptions)
    if (refreshDtExpiredOrNull) {
        window.location.href = AuthRoutes.Login;
        removeRefreshTokenDateTime();
        setCurrentStepId(0);
    }
    // if (window.location.href.includes("sign-up") || window.location.href.includes("login") && !api.endpoint.includes("loadUser"))
    return await refreshOrFailRequestUsingMutex(args, api, extraOptions)


}

export default customFetchBaseQuery;