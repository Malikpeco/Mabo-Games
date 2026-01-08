import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {StorefrontComponent} from './storefront/storefront.component';
import {SearchProductsComponent} from './search-products/search-products.component';
import {SharedModule} from '../shared/shared-module';


@NgModule({
  declarations: [
    StorefrontComponent,
    SearchProductsComponent,
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
  ]
})
export class PublicModule { }
