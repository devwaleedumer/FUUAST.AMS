import { ImageUploaderValues } from '@/lib/SchemaValidators/Image.Validator';
import { Faculty } from "@/types/faculty";
import { apiSlice } from "../apiSlice";
import { Department } from "@/types/department";
import { omitProps } from '@/lib/utils';

export const feeChallanApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getFeeChallan: builder.query({
            query: (id) => ({
                url: `FeeChallans/get-challan/${id}`,
                method: "GET",
                responseHandler: async (response) => URL.createObjectURL(await response.blob()), // Handle the response as a Blob
                cache: 'no-cache'
            }),
        }),

        uploadChallanImage: builder.mutation<unknown, { feeChallanId: number, data: ImageUploaderValues }>({
            query: ({ feeChallanId, data }) => {
                return {
                    url: `FeeChallans/upload-challan/${feeChallanId}`,
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: {
                        imageRequest: data.imageRequest
                    },
                    invalidatesTags: ["applicationForms"]


                };
            }
        })

    })
})

export const { useLazyGetFeeChallanQuery, useUploadChallanImageMutation } = feeChallanApi;