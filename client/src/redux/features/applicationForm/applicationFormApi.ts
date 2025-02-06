import { apiSlice } from "../apiSlice";
import { ApplicantDashboardResponse, ApplicationDetailResponse, ApplicationFormCreateRequest, ApplicationFormCreateResponse, SubmitApplicationFormResponse } from "@/types/applicationForm";
import { admissionSelectionValues, editAdmissionSelectionValues } from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";

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
                responseHandler: "text",
            }),
            invalidatesTags: ["applicationForms"]
        }),
        getSubmittedApplication: builder.query<SubmitApplicationFormResponse, null>({
            query: (id) => ({
                url: "/ApplicationForms/application-programs",
                method: "GET",
            }),
            providesTags: ["applicationForms"]
        }),
        getApplicationDetail: builder.query<ApplicationDetailResponse, number>({
            query: (id) => ({
                url: "/ApplicationForms/get-details/" + id,
                method: "GET",
            }),
            providesTags: ["applicationForms"]
        }),
        editSubmittedApplication: builder.mutation<string, editAdmissionSelectionValues>({
            query: (data) => ({
                url: `/ApplicationForms/application-programs/${data.id}`,
                method: "PUT",
                body: data,
                headers: {
                    'Content-Type': 'application/json',
                },
                responseHandler: "text",
            }),
            invalidatesTags: ["applicationForms"]
        }),
        getApplicantDashboardData: builder.query<ApplicantDashboardResponse, number>({
            query: (userId) => ({
                url: `/ApplicationForms/${userId}/applicant-dashboard-status`,
                method: "GET",
            }),
            providesTags: ["applicationForms"],
        }),
    }),

    overrideExisting: true
})
export const {
    useCreateApplicationFormMutation,
    useSubmitApplicationFormMutation,
    useGetSubmittedApplicationQuery,
    useEditSubmittedApplicationMutation,
    useGetApplicantDashboardDataQuery,
    useGetApplicationDetailQuery
} = applicationFormApi;