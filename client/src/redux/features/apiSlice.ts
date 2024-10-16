import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { userLoggedIn } from './auth/userSlice'
import { IUser } from '@/types/auth'
import customFetchBaseQuery from '@/lib/services/customBaseFetch'

export const apiSlice = createApi({
    reducerPath: "api",
    tagTypes: ["user", "applicant/personal-information", "program", "degreeGroup", "applicant/degrees", "faculty", "department", "applicationForms"],
    baseQuery: customFetchBaseQuery,
    endpoints: (builder) => ({
        refreshToken: builder.query({
            query: (data) => ({
                url: "tokens/refresh",
                method: "GET",
                credentials: "include" as const
            })
        }),
        loadUser: builder.query<IUser, null>({
            query: (data) => ({
                url: "users/me",
                method: "GET",
                credentials: "include" as const
            }),
            providesTags: ["user"],
            async onQueryStarted(arg, { queryFulfilled, dispatch }) {
                try {
                    const result = await queryFulfilled
                    dispatch(userLoggedIn({
                        user: result.data,
                        isAuthenticated: true
                    }))
                } catch (error) {
                    console.log(error)
                }
            }
        })
    }),
})
export const { useRefreshTokenQuery, useLoadUserQuery } = apiSlice;
