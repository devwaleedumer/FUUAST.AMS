import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login.component';
import {MessageModule} from 'primeng/message';

const routes: Routes = [{
  path:'',component:LoginComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes),MessageModule],
  exports: [RouterModule]
})
export class LoginRoutingModule { }
