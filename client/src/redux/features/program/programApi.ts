import { IProgram } from "@/types/program";
import { apiSlice } from "../apiSlice";

export const programApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getAllPrograms: builder.query<Array<IProgram>, null>({
            query: () => ({
                url: "programs",
                method: "GET"
            }),
            providesTags: ["program"]
        })
    })
})

export const { useGetAllProgramsQuery } = programApi;