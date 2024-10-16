import { IDegreeGroupWithDegreeLevel } from "@/types/degreeGroup";
import { apiSlice } from "../apiSlice";
import { Shift } from "@/types/shift";

export const departmentApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getTimeShiftByDepartmentId: builder.query<Array<Shift>, { departmentId: number, programId: number }>({
            query: ({ departmentId, programId }) => ({
                url: `departments/${departmentId}/${programId}/shifts`,
                method: "GET"
            }),
            providesTags: ["department"]
        })
    })
})

export const { useLazyGetTimeShiftByDepartmentIdQuery } = departmentApi;