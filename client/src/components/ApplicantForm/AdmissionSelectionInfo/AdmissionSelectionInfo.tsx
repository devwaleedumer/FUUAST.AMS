import { useAppSelector } from '@/hooks/reduxHooks'
import { useGetProgramByUserIdQuery } from '@/redux/features/applicant/applicantApi'
import { useGetAllFacultiesQuery } from '@/redux/features/faculity/faculityApi'
import { RootState } from '@/redux/store'
import React, { FC, useEffect } from 'react'
import { Heading } from "../../ui/heading";
import CreateAdmissionSelectionsInfo from './CreateAdmissionSelectionsInfo'
import { useGetSubmittedApplicationQuery } from '@/redux/features/applicationForm/applicationFormApi'
import PageLoader from '@/components/shared/Loader'
import EditAdmissionSelectionInfo from './EditAdmissionSelectionInfo'


type AdmissionSelectionInfoProps = {}
const title = "Edit ADMISSION INFORMATION & ORDER OF CHOICE";
const description =
  "Choose Discipline (Subject or Department) names and other related information in order of your choice";
const AdmissionSelectionInfo:FC<AdmissionSelectionInfoProps> = ({}) => {
  const {data: facultyData, isLoading: facultyIsLoading} = useGetAllFacultiesQuery(null);
  const id = useAppSelector((state : RootState) => state.auth?.user?.id);
  const {data: programData,isLoading : programDataIsLoading } = useGetProgramByUserIdQuery(Number(id));
  const {data: applicationFormData, isLoading: applicationFormIsLoading} = useGetSubmittedApplicationQuery(null);
  
  return (
  <>
  <Heading title={title} description={description} />
    {
        (!programData || !facultyData || applicationFormIsLoading) 
        ? <PageLoader/> : applicationFormData?.programsApplied.length! > 0 
        ? <EditAdmissionSelectionInfo facultyData={facultyData} data={applicationFormData!} /> 
        : <CreateAdmissionSelectionsInfo facultyData={facultyData} programId={programData.id}/>
    }
  </>
  )
}

export default AdmissionSelectionInfo