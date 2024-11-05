import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProgramtypeComponent } from './programtype.component';

const routes: Routes = [{
  path:"",component:ProgramtypeComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProgramtypeRoutingModule { }
