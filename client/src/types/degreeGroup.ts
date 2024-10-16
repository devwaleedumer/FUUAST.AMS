export interface IDegreeGroup {
    id: number;
    degreeName: string;
    degreeLevelId: number;
}
export interface IDegreeGroupWithDegreeLevel {
    [key: string]: IDegreeGroup[]
}