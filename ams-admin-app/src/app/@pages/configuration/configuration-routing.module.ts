import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

@NgModule({
  imports: [RouterModule.forChild([
    { path: 'faculty', loadChildren: () => import('./faculty/faculty.module').then(m => m.FacultyModule) },
    { path: 'session', loadChildren: () => import('./session/session.module').then(m => m.SessionModule) },
    { path: 'shift', loadChildren: () => import('./shift/shift.module').then(m => m.ShiftModule) },
    { path: 'programtype', loadChildren: () => import('./programtype/programtype.module').then(m => m.ProgramtypeModule) },
    { path: 'program', loadChildren: () => import('./program/program.module').then(m => m.ProgramModule) },
    { path: 'department', loadChildren: () => import('./department/department.module').then(m => m.DepartmentModule) },
    { path: 'academicyear', loadChildren: () => import('./academic-year/academic-year.module').then(m => m.AcademicYearModule) },
  ])],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
