import { toast } from "@/components/ui/use-toast";
import { isRejectedWithValue, Middleware, MiddlewareAPI } from "@reduxjs/toolkit";
import { HttpStatusCode } from "axios";
export const rtkQueryErrorLogger: Middleware =
    (api: MiddlewareAPI) => (next) => (action) => {
        // RTK Query uses `createAsyncThunk` from redux-toolkit under the hood, so we're able to utilize these matchers!
        if (isRejectedWithValue(action)) {
            const errorPayload = action.payload as any
            const httpCode = Number(errorPayload.status);
            if (!!httpCode)
                switch (httpCode) {
                    case HttpStatusCode.Unauthorized:
                        toast({
                            title: 'Authentication error!',
                            description:
                                'data' in errorPayload
                                    ? (errorPayload.data as { detail: string }).detail
                                    : "Login failed, try again",
                            variant: "destructive"


                        })
                        break;
                    case HttpStatusCode.BadRequest:
                        toast({
                            title: 'Error',
                            description:
                                'data' in errorPayload
                                    ? (errorPayload.data as { detail: string })?.detail || (errorPayload.data as { message: string })?.message
                                    : "some thing went wrong",
                        })
                        break;
                    case HttpStatusCode.NotFound:
                        // toast({
                        //     title: 'Error',
                        //     description:
                        //         'data' in errorPayload
                        //             ? (errorPayload.data as { detail: string })?.detail || (errorPayload.data as { message: string })?.message
                        //             : "some thing went wrong",
                        // })
                        break;
                    case HttpStatusCode.UnprocessableEntity:
                        const errorData = action.payload as any
                        let errorMessage: string[] = [];
                        Object.entries(errorData.data).map(([key, value]) => {
                            if (key.includes('.')) {
                                errorMessage.push(`${key.split('.').shift()} ${value} `)
                            } else {
                                errorMessage.push(`${value} `)
                            }
                        })
                        const formattedErrors = errorMessage.join("\n")
                        toast({
                            duration: 10000,
                            title: "invalid data format, re-enter and try again",
                            description: formattedErrors,
                            variant: "destructive"
                        })
                        break
                    default:
                        toast({
                            title: 'error',
                            description:
                                'data' in action.error
                                    ? (action.error.data as { detail: string }).detail
                                    : "Something went wrong try later!",
                            variant: "destructive"
                        })
                        break;
                }
        }

        return next(action)
    }
