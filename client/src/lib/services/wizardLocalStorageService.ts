import { IProgram } from "@/types/program"

const getCurrentStepId = () =>
    Number(localStorage?.getItem("currentStep") || 0)

const setCurrentStepId = (currentStep: number) => localStorage?.setItem("currentStep", String(currentStep))
const clearCurrentStepId = () => localStorage?.removeItem("currentStep")

const getSelectedProgram = () => localStorage?.getItem("selectedProgram") || ''
const setSelectedProgram = (id: string) => localStorage?.setItem("selectedProgram", id)
const clearSelectedProgram = () => localStorage?.removeItem("selectedProgram")


export { getCurrentStepId, setCurrentStepId, clearCurrentStepId, getSelectedProgram, setSelectedProgram, clearSelectedProgram }