import { Component, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { MessageService } from 'primeng/api';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    LoginRoutingModule
  ]
  ,
  providers:[MessageService]
})
export class LoginModule { }
