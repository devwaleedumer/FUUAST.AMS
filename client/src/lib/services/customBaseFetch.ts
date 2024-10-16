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

// create a new mutex
const mutex = new Mutex()
const baseQuery = fetchBaseQuery({
    baseUrl: process.env.NEXT_PUBLIC_SERVER_URI,
    credentials: "include" as const
})
const customFetchBaseQuery: BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> = async (args, api, extraOptions) => {
    const refreshTokenExpiryTime = getRefreshTokenDateTime()
    if (window.location.href.includes("login") && !api.endpoint.includes("loadUser"))
        return await baseQuery(args, api, extraOptions)

    if (refreshTokenExpiryTime == null) {
        const error: FetchBaseQueryError = {
            status: 401,
            data: {
                detail: "Session expired! login again"
            }
        }
        window.location.href = AuthRoutes.Login;
        return error;
    }
    if (Date.now() > new Date(refreshTokenExpiryTime as string).getTime()) {        // i want to return here
        removeRefreshTokenDateTime()
        window.location.href = AuthRoutes.Login;
    }
    // wait until the mutex is available without locking it
    await mutex.waitForUnlock()
    let result = await baseQuery(args, api, extraOptions)
    if (result.error && result.error.status === 401) {
        // checking whether the mutex is locked
        if (!mutex.isLocked()) {
            const release = await mutex.acquire()
            try {
                const refreshResult = await baseQuery(
                    '/tokens/refresh',
                    api,
                    extraOptions
                )
                if (refreshResult.data) {
                    // retry the initial query
                    setRefreshTokenDateTime((refreshResult?.data as ILoginResponse).refreshTokenExpiryTime)
                    result = await baseQuery(args, api, extraOptions)
                } else {
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

export default customFetchBaseQuery;