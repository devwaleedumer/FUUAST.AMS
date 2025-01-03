import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';

import { UserManagementRoutingModule } from './user-management-routing.module';
import { UserModule } from './user/user.module';
import { UserComponent } from './user/user.component';
import { MessageModule } from 'primeng/message';
import { ButtonDirective, ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService, PrimeTemplate } from 'primeng/api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ripple } from 'primeng/ripple';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { UserService } from '../../@core/services/user/user.service';
import { CheckboxModule } from 'primeng/checkbox';
import { DropdownModule } from 'primeng/dropdown';
import { RoleService } from '../../@core/services/role/role.service';
import { RoleComponent } from './role/role/role.component';
import { ProgressBarModule } from 'primeng/progressbar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
@NgModule({
  declarations: [
    UserComponent,
    RoleComponent
  ],
  imports: [
    CommonModule,
    UserManagementRoutingModule,
    UserModule,
    MessageModule,
    ButtonDirective,
    DialogModule,
    InputTextModule,
    PrimeTemplate,
    ReactiveFormsModule,
    Ripple,
    TableModule,
    ToastModule,
    ToolbarModule,
    TooltipModule,
    NgOptimizedImage,
    CheckboxModule,
    DropdownModule,
    ButtonModule,
    FormsModule,
    ProgressSpinnerModule
  ],
  providers: [MessageService, UserService, RoleService]
})
export class UserManagementModule { }
