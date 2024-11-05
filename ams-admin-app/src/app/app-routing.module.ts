import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppLayoutComponent } from './@layout/layout/layout.component';

@NgModule({
  imports: [
    RouterModule.forRoot([
      {
        path: 'applayout', component: AppLayoutComponent,
        children: [
          { path: '', loadChildren: () => import('./@components/dashboard/dashboard.module').then(m => m.DashboardModule) },
          { path: 'app', loadChildren: () => import('./@pages/pages.module').then(m => m.PagesModule) },
        //   { path: 'utilities', loadChildren: () => import('./demo/components/utilities/utilities.module').then(m => m.UtilitiesModule) },
        //   { path: 'documentation', loadChildren: () => import('./demo/components/documentation/documentation.module').then(m => m.DocumentationModule) },
        //   { path: 'blocks', loadChildren: () => import('./demo/components/primeblocks/primeblocks.module').then(m => m.PrimeBlocksModule) },
        //   { path: 'pages', loadChildren: () => import('./demo/components/pages/pages.module').then(m => m.PagesModule) }
        ],
      },
      {path:'',loadChildren: () => import('./@auth/auth.module').then(m => m.AuthModule) },
      {path:'auth',loadChildren: () => import('./@auth/auth.module').then(m => m.AuthModule) },
      // { path: 'auth', loadChildren: () => import('./demo/components/auth/auth.module').then(m => m.AuthModule) },
      // { path: 'landing', loadChildren: () => import('./demo/components/landing/landing.module').then(m => m.LandingModule) },
      // { path: 'notfound', component: NotfoundComponent },
      // { path: '**', redirectTo: '/notfound' },
    ], { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
