import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

import { AccessRoutingModule } from './pagenotfound-routing.module';
import { pagenotfoundComponent } from './pagenotfound.component';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        AccessRoutingModule,
        ButtonModule
    ],
    declarations: [pagenotfoundComponent]
})
export class AccessModule { }
