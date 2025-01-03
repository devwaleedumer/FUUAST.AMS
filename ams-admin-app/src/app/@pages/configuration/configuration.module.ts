import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfigurationRoutingModule } from './configuration-routing.module';

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
import { ShiftComponent } from './shift/shift.component';
import { ShiftService } from '../../@core/services/shift/shift.service';
import { FacultyComponent } from './faculty/faculty.component';
import { FacultyService } from '../../@core/services/faculty/faculty.service';
import { ProgramtypeComponent } from './programtype/programtype.component';
import { ProgramtypeService } from '../../@core/services/programtype/programtype.service';
import { ProgramComponent } from './program/program.component';
import { ProgramService } from '../../@core/services/program/program.service';
import { DepartmentComponent } from './department/department.component';
import { DepartmentService } from '../../@core/services/department/department.service';
import { AcademicYearComponent } from './academic-year/academic-year.component';
import { SessionService } from '../../@core/services/session/session.service';
import { AcademicyearService } from '../../@core/services/academicyear/academicyear.service';
import { SessionComponent } from './session/session.component';

@NgModule({
  declarations: [
    ShiftComponent,FacultyComponent,ProgramtypeComponent, ProgramComponent, DepartmentComponent, AcademicYearComponent,SessionComponent
  ],
  imports: [
    CommonModule,
    ConfigurationRoutingModule,
    TableModule,
    FileUploadModule,
    FormsModule,
    ButtonModule,
    RippleModule,
    ToastModule,
    ToolbarModule,
    RatingModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    RadioButtonModule,
    InputNumberModule,
    DialogModule,
    TooltipModule,
    ReactiveFormsModule

  ]
  ,
  providers:[MessageService,ShiftService,FacultyService,ProgramtypeService,ProgramService,DepartmentService,SessionService,AcademicyearService]
})
export class ConfigurationModule { }
