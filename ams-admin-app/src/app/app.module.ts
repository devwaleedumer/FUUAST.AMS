import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { AppLayoutModule } from './@layout/layout.module';
import { ProgressBarModule } from 'primeng/progressbar';
import { RouterModule } from '@angular/router';
import { AuthInterceptorModule } from './@core/interceptors/auth-interceptor/auth-interceptor.module';




@NgModule({
  declarations: [
    AppComponent,

  ],
  imports: [
    BrowserModule,
    AppLayoutModule,
    RouterModule,
    AppRoutingModule,
    AuthInterceptorModule,
    ProgressBarModule,
    
  ],
  providers: [{ provide: LocationStrategy, useClass: PathLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
