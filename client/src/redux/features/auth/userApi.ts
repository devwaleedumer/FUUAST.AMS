import { ILoginRequest, ILoginResponse } from "@/types/auth";
import { apiSlice } from "../apiSlice";
import { userLoggedIn, userLoggedOut } from "./userSlice";
import { createSessionCookies } from "@/lib/auth/tokenCookies";
import { api } from "@/lib/services/api";
import { setAuthorizationHeader } from "@/lib/services/interceptor";

type RegistrationResponse = {
    message: string;
    activationToken: string;
}
type RegistrationData = {}

export const userApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        register: builder.mutation<RegistrationResponse, RegistrationData>({
            query: (data) => ({
                url: "registration",
                method: "POST",
                body: data,
                credentials: "include" as const
            }),
            async onQueryStarted(arg, { queryFulfilled, dispatch }) {
                try {
                    const result = await queryFulfilled;
                    // dispatch(userRegistration({ token: result.data.activationToken }))
                } catch (error) {
                    console.log(error)
                }
            }
        }),
        activation: builder.mutation({
            query: ({ activationToken, activationCode }) => ({
                url: "activate-user",
                method: "POST",
                body: {
                    activationToken: activationToken,
                    activationCode: activationCode,
                }
            }),
        }),
        login: builder.mutation<ILoginResponse, ILoginRequest>({
            query: ({ email, password }) => ({
                url: "get-token",
                method: "POST",
                body: {
                    email,
                    password
                },
            }),
            async onQueryStarted(arg, { queryFulfilled, dispatch }) {
                try {
                    const result = await queryFulfilled;
                    const { refreshToken, accessToken, refreshTokenExpiryTime } = result.data
                    createSessionCookies({ refreshToken, refreshTokenExpiryTime, accessToken })
                    setAuthorizationHeader({ request: api.defaults, accessToken })
                } catch (error) {
                    console.log(error)
                }
            }
        }),
        logout: builder.query({
            query: ({ email, password }) => ({
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




export const { useRegisterMutation, useActivationMutation, useLoginMutation, useLogoutQuery } = userApi