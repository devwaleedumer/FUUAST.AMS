import { createSlice } from "@reduxjs/toolkit";
import { admissionSelectionValidator } from "@/lib/SchemaValidators/ApplicationForm/AdmissionSelectionsSchema.validator";
import { degreeValidator } from "@/lib/SchemaValidators/ApplicationForm/DegreeSchema.validator";
import { programValidator } from "@/lib/SchemaValidators/ApplicationForm/ProgramSchema.validator";
import { z } from "zod";
import { setCurrentStepId } from "@/lib/services/wizardLocalStorageService";
import { stat } from "fs";

type AdmissionSelection = z.infer<typeof admissionSelectionValidator>
type Degrees = z.infer<typeof degreeValidator>
// type PersonalInformation = z.infer<typeof personalInfo>
type ProgramTypeSelection = z.infer<typeof programValidator>

interface ApplicantFormWizardType {
    currentStep: number,
}

const initialState: ApplicantFormWizardType = {
    currentStep: 0,
};

const wizardSlice = createSlice({
    name: "applicationFormWizard",
    initialState,
    reducers: {
        // Move to the next step
        nextStep: (state) => {
            if (state.currentStep <= 3) {
                state.currentStep += 1;
                setCurrentStepId(state.currentStep)
            }

        },
        // Move to the previous step
        prevStep: (state) => {
            if (state.currentStep >= 0) {
                state.currentStep -= 1;
                setCurrentStepId(state.currentStep)
            }
        },
        resetState: (state) => {
            state.currentStep = 0;
            setCurrentStepId(0);
        },
        initializeState: (state, actions) => {
            state.currentStep = actions.payload
        }

    }
})
export const { nextStep, prevStep, initializeState, resetState } = wizardSlice.actions;
export default wizardSlice.reducer;