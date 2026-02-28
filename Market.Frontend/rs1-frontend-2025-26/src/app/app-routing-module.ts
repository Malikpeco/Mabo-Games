import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {myAuthData, myAuthGuard} from './core/guards/my-auth-guard';

const routes: Routes = [

  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: '',
    loadChildren: () =>
      import('./modules/public/public-module').then(m => m.PublicModule)
  },
  // fallback 404
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'top'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
