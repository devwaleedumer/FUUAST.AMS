import { ILoginRequest, ILoginResponse, IRegisterRequest, IRegisterResponse } from "@/types/auth";
import { apiSlice } from "../apiSlice";
import { userLoggedOut } from "./userSlice";
import { setRefreshTokenDateTime } from "@/lib/services/authLocalStorageService";


export const userApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        register: builder.mutation<string, IRegisterRequest>({
            query: (data) => ({
                url: "accounts/register",
                method: "POST",
                body: data,
            }),

        }),
        verifyEmail: builder.query<string, { userId: string, code: string, cnic: string, fullName: string }>({
            query: ({ userId, code, cnic, fullName }) => ({
                url: `accounts/confirm-email?userId=${userId}&code=${code}&cnic=${cnic}&fullName=${fullName}`,
                method: "GET",
                responseHandler: "text"

            })
        }),
        changePassword: builder.mutation<string, { password: string, newPassword: string, confirmNewPassword: string }>({
            query: (data) => ({
                url: "accounts/change-password",
                method: "POST",
                body: data,
                responseHandler: "text"
            })
        }),
        login: builder.mutation<ILoginResponse, ILoginRequest>({
            query: ({ email, password }) => ({
                url: "tokens/get-token-cookie",
                method: "POST",
                body: {
                    email,
                    password
                },
                credentials: "include" as const
            }),
            async onQueryStarted(arg, { queryFulfilled, dispatch }) {
                try {
                    const result = await queryFulfilled;
                    const { refreshTokenExpiryTime } = result?.data
                    apiSlice.endpoints.loadUser.initiate(null);
                    setRefreshTokenDateTime(refreshTokenExpiryTime)
                } catch (error) {
                    console.log(error)
                }
            }
        }),
        logout: builder.query({
            query: () => ({
                url: "tokens/logout-cookie",
                method: "GET",
                credentials: "include" as const
            }),
        }),
    }),
    overrideExisting: true
})




export const { useRegisterMutation, useLoginMutation, useLazyLogoutQuery, useVerifyEmailQuery, useChangePasswordMutation } = userApi