import { Faculty } from "@/types/faculty";
import { apiSlice } from "../apiSlice";
import { Department } from "@/types/department";

export const facultyAPI = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getAllFaculties: builder.query<Array<Faculty>, null>({
            query: () => ({
                url: "faculty",
                method: "GET"
            }),
            providesTags: ["faculty"]
        }),
        getDepartmentsByFacultyId: builder.query<Array<Department>, { facultyId: number, programId: number }>({
            query: ({ facultyId, programId }) => ({
                url: `faculty/${facultyId}/${programId}/departments`,
                method: "GET"
            }),
            providesTags: ["faculty"]
        }),
    })
})

export const { useGetAllFacultiesQuery, useLazyGetDepartmentsByFacultyIdQuery } = facultyAPI;