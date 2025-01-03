import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LocationStrategy, PathLocationStrategy } from '@angular/common';
import { AppLayoutModule } from './@layout/layout.module';
import { AuthInterceptor } from './@core/interceptors/auth-interceptor/auth-interceptor.interceptor';
import { AuthInterceptorModule } from './@core/interceptors/auth-interceptor/auth-interceptor.module';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { ProgressBarModule } from 'primeng/progressbar';
@NgModule({
  declarations: [
    AppComponent,

  ],
  imports: [
    BrowserModule,
    AppLayoutModule,
    AppRoutingModule,
    AuthInterceptorModule,
    ProgressBarModule,


  ],
  providers: [{ provide: LocationStrategy, useClass: PathLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
