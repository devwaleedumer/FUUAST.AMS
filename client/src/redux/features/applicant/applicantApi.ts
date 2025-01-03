import { ApplicantDegrees, Degree, PersonalInformation } from "@/types/applicant";
import { apiSlice } from "../apiSlice";
import { DegreeValues } from "@/lib/SchemaValidators/ApplicationForm/DegreeSchema.validator";
import { IProgram } from "@/types/program";
import { PersonalEditInfoValues, PersonalInfoValues } from "@/lib/SchemaValidators/ApplicationForm/PersonalInfoSchema.Validator";

export const applicantApi = apiSlice.injectEndpoints({

    endpoints: (builder) => ({
        getApplicantPersonalInformation: builder.query<PersonalInformation, null>({
            query: () => ({
                url: "applicants/personal-information",
                method: "GET",
            }),
            providesTags: ["applicant/personal-information"],
        }),
        createApplicantPersonalInformation: builder.mutation<PersonalInformation, PersonalInfoValues>({
            query: (data) => ({
                url: "applicants/personal-information",
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: data
            }),
            invalidatesTags: ["applicant/personal-information", "user"]
        }),
        editApplicantPersonalInformation: builder.mutation<PersonalInformation, PersonalEditInfoValues>({
            query: (data) => ({
                url: "applicants/personal-information/" + data.id,
                method: "PUT",
                headers: {
                    'Content-Type': "application/json"
                },
                body: data
            }),
            invalidatesTags: ["applicant/personal-information", "user"]
        }),
        createApplicantDegrees: builder.mutation<Degree, DegreeValues>({
            query: (data) => ({
                url: "applicants/degrees",
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: data
            }),
            invalidatesTags: ["applicant/degrees",]
        }),
        editApplicantDegrees: builder.mutation<Degree, DegreeValues>({
            query: (data) => ({
                url: "applicants/degrees",
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: data
            }),
            invalidatesTags: ["applicant/degrees"]
        }),
        getApplicantDegrees: builder.query<Array<Degree>, null>({
            query: () => ({
                url: "applicants/degrees",
                method: "GET",
            }),
            providesTags: ["applicant/degrees"]
        }),
        getProgramByUserId: builder.query<IProgram, number>({
            query: (applicantId) => ({
                url: `Users/${applicantId}/programs`,
                method: "GET"
            }),
            providesTags: ["program"]
        })
    })
})

export const {
    useCreateApplicantPersonalInformationMutation,
    useGetApplicantPersonalInformationQuery,
    useEditApplicantPersonalInformationMutation,
    useCreateApplicantDegreesMutation,
    useGetApplicantDegreesQuery,
    useEditApplicantDegreesMutation,
    useGetProgramByUserIdQuery
}
    = applicantApi