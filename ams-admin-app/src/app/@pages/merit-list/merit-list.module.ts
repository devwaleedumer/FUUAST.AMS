import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { TooltipModule } from 'primeng/tooltip';
import { ToolbarModule } from 'primeng/toolbar';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { MeritListRoutingModule } from './merit-list-routing.module';
import { MeritListComponent } from './merit-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';

@NgModule({
  declarations: [MeritListComponent],
  imports: [
    TableModule,
    ButtonModule,
    ToastModule,
    ToolbarModule,
    TooltipModule,
    CommonModule,
    MeritListRoutingModule,
    DialogModule,
    ReactiveFormsModule,
    DropdownModule,
    InputTextModule
  ]
})
export class MeritListModule { }
