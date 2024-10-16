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
            })
        }),
        login: builder.mutation<ILoginResponse, ILoginRequest>({
            query: ({ email, password }) => ({
                url: "tokens/get-token",
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
                url: "logout",
                method: "GET",
                credentials: "include" as const
            }),
            async onQueryStarted(arg, { queryFulfilled, dispatch }) {
                try {
                    dispatch(userLoggedOut())
                } catch (error) {
                    console.log(error)
                }
            }
        }),

    })
})




export const { useRegisterMutation, useLoginMutation, useLogoutQuery } = userApi