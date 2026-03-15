import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {StorefrontComponent} from './storefront/storefront.component';
import {SharedModule} from '../shared/shared-module';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { FormsModule } from '@angular/forms';
import { GameDetailsComponent } from './game-details/game-details.component';
import { CartComponent } from './cart/cart.component';
import { BeginCheckoutComponent } from './begin-checkout/begin-checkout.component';
import { PaymentComponent } from './payment/payment.component';
import { LibraryComponent } from './library/library.component';


@NgModule({
  declarations: [
    StorefrontComponent,
    BrowseGamesComponent,
    GameDetailsComponent,
    CartComponent,
    BeginCheckoutComponent,
    PaymentComponent,
    LibraryComponent,
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
    FormsModule,
  ]
})
export class PublicModule { }
