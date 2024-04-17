import {
  ApplicationForm,
  FormContextType,
  initializeApplicationForm,
} from "@/types";
import { FC, ReactNode, createContext, useState } from "react";
export const FormContext = createContext<FormContextType>(null!);

type FormContextProviderProps = {
  children: ReactNode;
};
export const FormContextProvider: FC<FormContextProviderProps> = ({
  children,
}) => {
  const [step1Answered, setStep1Answered] = useState<boolean>(false);
  const [step2Answered, setStep2Answered] = useState<boolean>(false);
  const [step3Answered, setStep3Answered] = useState<boolean>(false);
  const [finished, setFinished] = useState<boolean>(false);
  const [stepData, setStepData] = useState<ApplicationForm>(
    initializeApplicationForm
  );
  const formContextValues = {
    step1Answered,
    setStep1Answered,
    step2Answered,
    setStep2Answered,
    step3Answered,
    setStep3Answered,

    finished,
    setFinished,
    stepData,
    setStepData,
  };
  return (
    <div>
      <FormContext.Provider value={formContextValues}>
        {children}
      </FormContext.Provider>
    </div>
  );
};
