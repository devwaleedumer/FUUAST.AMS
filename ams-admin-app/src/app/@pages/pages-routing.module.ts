import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [RouterModule.forChild([
        { path: 'configuration', loadChildren: () => import('./configuration/configuration.module').then(m => m.ConfigurationModule) },
        { path: 'user-management', loadChildren: () => import('./user-management/user-management.module').then(m => m.UserManagementModule) },
        // { path: 'empty', loadChildren: () => import('./empty/emptydemo.module').then(m => m.EmptyDemoModule) },
        // { path: 'timeline', loadChildren: () => import('./timeline/timelinedemo.module').then(m => m.TimelineDemoModule) },
        // { path: '**', redirectTo: '/notfound' }
        { path: 'application', loadChildren: () => import('./application/application.module').then(m => m.ApplicationModule) },
    ])],
    exports: [RouterModule]
})
export class PagesRoutingModule { }
