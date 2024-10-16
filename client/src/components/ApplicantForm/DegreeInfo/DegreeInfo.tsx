import React, { FC, useEffect, useMemo, useState } from "react";
import { Heading } from "../../ui/heading";
import { academicDegreeNamesRecord, AcademicDegreeNamesRecordParam } from "@/lib/data";
import { getSelectedProgram } from "@/lib/services/wizardLocalStorageService";
import { useGetAllDegreeGroupByDegreeLevelDictionaryQuery } from "@/redux/features/degreeGroup/degreeGroupApi";
import { IDegreeGroupWithDegreeLevel } from "@/types/degreeGroup";
import CreateDegree from "./CreateDegree";
import { useGetApplicantDegreesQuery, useGetProgramByUserIdQuery } from "@/redux/features/applicant/applicantApi";
import EditDegree from "./EditDegree";
import { ApplicantDegrees, Degree } from "@/types/applicant";
import { useAppSelector } from "@/hooks/reduxHooks";
import { RootState } from "@/redux/store";

type DegreeInfoProps = {};
const title = "Academic Degrees";
const description = "Fill up info related to Previous Academic degrees";
const DegreeInfo: FC<DegreeInfoProps> = () => {
  const [isMounted, setIsMounted] = useState<boolean>(false);
  const id = useAppSelector((state : RootState) => state.auth?.user?.id)
  const {data: programData,isLoading: programIsLoading } = useGetProgramByUserIdQuery(Number(id))
  const {data,isLoading : degreeGroupIsLoading} = useGetAllDegreeGroupByDegreeLevelDictionaryQuery(null);
  const {data: editData, isSuccess:editIsSuccess, isLoading: applicantIsLoading } = useGetApplicantDegreesQuery(null);
  useEffect(() => {
    setIsMounted(true)
  },[])
  const isLoading = degreeGroupIsLoading || applicantIsLoading || programIsLoading;
const degreesNameList = useMemo(() => academicDegreeNamesRecord[String(programData?.id) as AcademicDegreeNamesRecordParam], [programData])
  return (
   (isMounted && !isLoading) && 
   <>
      <Heading title={title} description={description} />
      { editIsSuccess && editData?.length > 0  ?  
        <EditDegree
         degreeList={degreesNameList} 
         appliedProgram={String(programData?.id)} 
         degreeGroupData={data as IDegreeGroupWithDegreeLevel}
         editData={editData as Degree[]}
         />
          :
          <CreateDegree
         degreeList={degreesNameList} 
         appliedProgram={String(programData?.id)} 
         degreeGroupData={data as IDegreeGroupWithDegreeLevel}/>
          }     
    </>
  );
};
//todo

export default DegreeInfo;
