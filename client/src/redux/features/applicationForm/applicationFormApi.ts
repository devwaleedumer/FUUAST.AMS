import { apiSlice } from "../apiSlice";
import { ApplicationFormCreateRequest, ApplicationFormCreateResponse } from "@/types/applicationForm";
import { admissionSelectionValues } from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";

export const applicationFormApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        createApplicationForm: builder.mutation<ApplicationFormCreateResponse, ApplicationFormCreateRequest>({
            query: (data) => ({
                url: "/applicationForms",
                method: "POST",
                body: data,
                headers: {
                    'Content-Type': 'application/json',
                },
            }),
            invalidatesTags: ["applicationForms"]
        }),
        submitApplicationForm: builder.mutation<string, admissionSelectionValues>({
            query: (data) => ({
                url: "/ApplicationForms",
                method: "PUT",
                body: data,
                headers: {
                    'Content-Type': 'application/json',
                },
            })
        })
    })
})
export const { useCreateApplicationFormMutation, useSubmitApplicationFormMutation } = applicationFormApi;