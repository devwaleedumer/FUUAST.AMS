import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forChild([{
     path: 'applicant', loadChildren: () => import('./applicant-detail/applicant-detail.module').then(m => m.ApplicantDetailModule) }
  ])],
  exports: [RouterModule]
})
export class ApplicationRoutingModule { }
