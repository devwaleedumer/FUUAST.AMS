// import {
//     AxiosDefaults,
//     AxiosError,
//     AxiosInstance,
//     AxiosRequestConfig,
//     AxiosResponse,
//     InternalAxiosRequestConfig
// } from 'axios'
// import {
//     createSessionCookies,
//     getRefreshToken,
//     getToken,
//     removeSessionCookies
// } from '@/lib/auth/tokenCookies'
// import paths from '@/utils/constants/path.Constant'
// import { apiAxios } from './api'

// // queueing failed requests which were failed due to refresh token expiry
// type FailedRequestQueue = {
//     onSuccess: (accessToken: string) => void
//     onFailure: (error: AxiosError) => void
// }

// let isRefreshing = false
// let failedRequestQueue: FailedRequestQueue[] = []
// // authorize header function params
// type SetAuthorizationHeaderParams = {
//     request: AxiosDefaults | AxiosRequestConfig
//     accessToken: string
// }

// export function setAuthorizationHeader(params: SetAuthorizationHeaderParams) {
//     const { request, accessToken } = params

//         // set authorization bearer
//         ; (request.headers as Record<string, unknown>)[
//             'Authorization'
//         ] = `Bearer ${accessToken}`
// }

// // handle token refresh
// function handleRefreshToken(refreshToken: string) {
//     isRefreshing = true
//     apiAxios
//         .post(
//             '/refresh',
//             { refreshToken },
//             {
//                 headers: {
//                     Authorization: `Bearer ${getToken()}`
//                 }
//             }
//         )
//         .then((response: any) => {
//             const { accessToken } = response.data
//             // store tokens in cookies and set authorization header
//             createSessionCookies({ accessToken, refreshToken: response.data.refreshToken })
//             setAuthorizationHeader({ request: apiAxios.defaults, accessToken })
//             // on refresh success
//             failedRequestQueue.forEach((request) => request.onSuccess(accessToken))
//             failedRequestQueue = []
//         })
//         .catch((error: any) => {
//             // on refresh failure
//             failedRequestQueue.forEach((request) => request.onFailure(error))
//             failedRequestQueue = []
//             removeSessionCookies()
//         })
//         .finally(() => {
//             isRefreshing = false
//         })
// }
// // onRequest modify request and return updated configs
// function onRequest(config: AxiosRequestConfig) {
//     const accessToken = getToken()

//     if (accessToken) {
//         setAuthorizationHeader({ request: config, accessToken })
//     }

//     return config as InternalAxiosRequestConfig
// }
// // reject promise with error/cause
// function onRequestError(error: AxiosError): Promise<AxiosError> {
//     return Promise.reject(error)
// }
// // onResponse modify response data and return updated data
// function onResponse(response: AxiosResponse): AxiosResponse {
//     return response
// }

// type ErrorCode = {
//     code: string
// }

// function onResponseError(error: AxiosError<ErrorCode>
// ): Promise<AxiosError | AxiosResponse> {
//     if (error?.response?.status === 401) {
//         if (error.response?.data?.code === 'token.expired') {
//             const originalConfig = error.config as AxiosRequestConfig
//             const refreshToken = getRefreshToken()

//             if (!isRefreshing) {
//                 handleRefreshToken(refreshToken)
//             }

//             return new Promise((resolve, reject) => {
//                 failedRequestQueue.push({
//                     onSuccess: (accessToken: string) => {
//                         setAuthorizationHeader({ request: originalConfig, accessToken })
//                         resolve(apiAxios(originalConfig))
//                     },
//                     onFailure: (error: AxiosError) => {
//                         reject(error)
//                     }
//                 })
//             })
//         } else {
//             removeSessionCookies()
//             window.location.href = paths.LOGIN_PATH
//         }
//     }

//     return Promise.reject(error)
// }

// export function setupInterceptors(axiosInstance: AxiosInstance): AxiosInstance {
//     axiosInstance.interceptors.request.use(onRequest, onRequestError)
//     axiosInstance.interceptors.response.use(onResponse, onResponseError)

//     return axiosInstance
// }