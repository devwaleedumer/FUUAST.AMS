import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplicantDetailComponent } from './applicant-detail.component';

const routes: Routes = [{
  path:'',component:ApplicantDetailComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicantDetailRoutingModule { }
