import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


@NgModule({
  imports: [RouterModule.forChild([
    { path: 'faculty', loadChildren: () => import('./faculty/faculty.module').then(m => m.FacultyModule) },

  ])],
  exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
