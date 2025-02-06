import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesRoutingModule } from './pages-routing.module';
import { UserManagementModule } from './user-management/user-management.module';
import { AccountModule } from './account/account.module';
import { NotfoundComponent } from './notfound/notfound.component';
import { MeritListModule } from './merit-list/merit-list.module';
@NgModule({
  declarations: [NotfoundComponent],
  imports: [
    CommonModule,
    PagesRoutingModule,
    UserManagementModule,
    AccountModule,
    MeritListModule
  ]
})
export class PagesModule { }
