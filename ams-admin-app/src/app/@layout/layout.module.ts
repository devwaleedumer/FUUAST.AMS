import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputTextModule } from 'primeng/inputtext';
import { SidebarModule } from 'primeng/sidebar';
import { BadgeModule } from 'primeng/badge';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputSwitchModule } from 'primeng/inputswitch';
import { RippleModule } from 'primeng/ripple';
import { AppMenuItemComponent } from './sidebar/menuitem/menuitem.component';
import { HeaderComponent } from './header/header.component';
import { AppMenuComponent } from './sidebar/menu/menu.component';
import { AppFooterComponent } from './footer/footer.component';
import { AppSidebarComponent } from './sidebar/sidebar.component';
import { AppLayoutComponent } from './layout/layout.component';
import { RouterModule } from '@angular/router';
import { AppConfigModule } from './config/config.module';


@NgModule({
  declarations: [
    AppMenuItemComponent,
    HeaderComponent,
    AppFooterComponent,
    AppMenuComponent,
    AppSidebarComponent,
    AppLayoutComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    InputTextModule,
    SidebarModule,
    BadgeModule,
    RadioButtonModule,
    InputSwitchModule,
    RippleModule,
    RouterModule,
    AppConfigModule
  ],
  exports: [AppLayoutComponent],
  providers: [provideHttpClient()],
})
export class AppLayoutModule { }
