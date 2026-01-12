import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {StorefrontComponent} from './storefront/storefront.component';
import {SharedModule} from '../shared/shared-module';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    StorefrontComponent,
    BrowseGamesComponent,
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
    FormsModule,
  ]
})
export class PublicModule { }
