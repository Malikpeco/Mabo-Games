import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StorefrontComponent } from './storefront/storefront.component';
import { SearchProductsComponent } from './search-products/search-products.component';

const routes: Routes = [
  {
    path: '',
    component: StorefrontComponent,
    children: [
      { path: '**', redirectTo: '' }//if user goes to any url that doesnt exist, send them back to '/'
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {}
