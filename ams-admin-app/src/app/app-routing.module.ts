import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppLayoutComponent } from './@layout/layout/layout.component';
import { authGuard } from './@core/guard/auth.guard';
import { NotfoundComponent } from './@pages/notfound/notfound.component';
import { LoadingService } from './@core/services/loading/loading.service';
import { pagenotfoundComponent } from './@components/404NotFound/pagenotfound.component';

@NgModule({
  imports: [
    RouterModule.forRoot([
      {
        path: 'applayout', component: AppLayoutComponent,
        canActivate: [authGuard],
        children: [
          { path: '', loadChildren: () => import('./@components/dashboard/dashboard.module').then(m => m.DashboardModule) },
          { path: 'app', loadChildren: () => import('./@pages/pages.module').then(m => m.PagesModule) },
          //   { path: 'utilities', loadChildren: () => import('./demo/components/utilities/utilities.module').then(m => m.UtilitiesModule) },
          //   { path: 'documentation', loadChildren: () => import('./demo/components/documentation/documentation.module').then(m => m.DocumentationModule) },
          //   { path: 'blocks', loadChildren: () => import('./demo/components/primeblocks/primeblocks.module').then(m => m.PrimeBlocksModule) },
          //   { path: 'pages', loadChildren: () => import('./demo/components/pages/pages.module').then(m => m.PagesModule) }
        ],
      },
      { path: '', loadChildren: () => import('./@auth/auth.module').then(m => m.AuthModule) },
      { path: 'auth', loadChildren: () => import('./@auth/auth.module').then(m => m.AuthModule) },
      { path: 'account', loadChildren: () => import('./@pages/account/account.module').then(m => m.AccountModule) },
      // { path: 'auth', loadChildren: () => import('./demo/components/auth/auth.module').then(m => m.AuthModule) },
      { path: 'notfound', component: NotfoundComponent },
      { path: '**', redirectTo: '/notfound' },

    ], { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
  ],
  exports: [RouterModule],
  providers: [LoadingService]
})
export class AppRoutingModule {
}
