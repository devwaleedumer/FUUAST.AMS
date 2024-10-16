import { IDegreeGroupWithDegreeLevel } from "@/types/degreeGroup";
import { apiSlice } from "../apiSlice";

export const degreeGroupApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        getAllDegreeGroupByDegreeLevelDictionary: builder.query<IDegreeGroupWithDegreeLevel, null>({
            query: () => ({
                url: "DegreeGroups/degree-levels",
                method: "GET"
            }),
            providesTags: ["degreeGroup"]
        })
    })
})

export const {  useGetAllDegreeGroupByDegreeLevelDictionaryQuery  } = degreeGroupApi;