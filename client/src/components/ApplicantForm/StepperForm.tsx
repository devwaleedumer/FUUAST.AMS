"use client";
import { useContext, useEffect, useRef, useState } from "react";
import AdmissionSelectionsInfo from "./AdmissionSelectionsInfo";
import DegreeInfo from "./DegreeInfo/DegreeInfo";
import PersonalInfo from "./PersonalInfo/PersonalInfo";
import ProgramInfo from "./ProgramInfo";
import { useAppDispatch, useAppSelector } from "@/hooks/reduxHooks";
import { RootState } from "@/redux/store";
import { initializeState } from "@/redux/features/applicant/applicationWizardSlice";
import { getCurrentStepId } from "@/lib/services/wizardLocalStorageService";
import LayoutLoader from "../shared/Loader";

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
   <AdmissionSelectionsInfo   key={4}/>
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
