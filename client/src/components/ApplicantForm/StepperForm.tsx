"use client";
import { useContext, useEffect, useRef, useState } from "react";
 const DegreeInfo =   dynamic(
  () => import("./DegreeInfo/DegreeInfo"),
  {
    loading: () =>  <PageLoader/>
  }
 )
 const PersonalInfo =   dynamic(
  () => import("./PersonalInfo/PersonalInfo"),
  {
    loading: () =>  <PageLoader/>
  }
 )
 const ProgramInfo =   dynamic(
  () => import("./ProgramInfo"),
  {
    loading: () =>  <PageLoader/>
  }
 )
 const AdmissionSelectionInfo =   dynamic(
  () => import("./AdmissionSelectionInfo/AdmissionSelectionInfo"),
  {
    loading: () =>  <PageLoader/>
  }
 )

import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks";
import { RootState } from "@/redux/store";
import { initializeState } from "@/redux/features/applicant/applicationWizardSlice";
import { getCurrentStepId } from "@/lib/services/wizardLocalStorageService";
import LayoutLoader from "../shared/Loader";
import dynamic from "next/dynamic";
import PageLoader from "../shared/Loader";
// import ProgramInfo from "./ProgramInfo";


interface ProfileFormType {
}

export const Application: React.FC<ProfileFormType> = ({

}) => {
  const [isComponentMounted, setIsComponentMounted] = useState<boolean>(false)
  const currentStep : number = useAppSelector((state: RootState) => state.wizard.currentStep);
  const dispatch = useAppDispatch();
 useEffect(() =>{
    setIsComponentMounted(true)
   dispatch(initializeState(getCurrentStepId()))
 },[])

 const comps = [
   <PersonalInfo   key={2} />,
   <ProgramInfo    key={1}/>,
   <DegreeInfo  key={3}/>,
   <AdmissionSelectionInfo   key={4}/>
 ]

  return (
 
 isComponentMounted ? (
    <>
      {comps[currentStep]}
    </> 
  )
  : <LayoutLoader/>
  );
};
