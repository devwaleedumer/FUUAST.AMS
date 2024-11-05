import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShiftRoutingModule } from './shift-routing.module';
import { ShiftComponent } from './shift.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { FileUploadModule } from 'primeng/fileupload';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import {ToolbarModule} from 'primeng/toolbar'
import { RatingModule } from 'primeng/rating';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { DialogModule } from 'primeng/dialog';
import {TooltipModule} from 'primeng/tooltip';
import { MessageService } from 'primeng/api';
import { ShiftService } from '../../../@core/services/shift/shift.service';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ShiftRoutingModule,
    // TableModule,
    // FileUploadModule,
    // FormsModule,
    // ButtonModule,
    // RippleModule,
    // ToastModule,
    // ToolbarModule,
    // RatingModule,
    // InputTextModule,
    // InputTextareaModule,
    // DropdownModule,
    // RadioButtonModule,
    // InputNumberModule,
    // DialogModule,
    // TooltipModule,
    // ReactiveFormsModule
  ],
  providers:[]
})
export class ShiftModule { }
