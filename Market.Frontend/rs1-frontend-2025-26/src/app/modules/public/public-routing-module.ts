import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StorefrontComponent } from './storefront/storefront.component';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { GameDetailsComponent } from './game-details/game-details.component';
import { CartComponent } from './cart/cart.component';
import { BeginCheckoutComponent } from './begin-checkout/begin-checkout.component';
import { PaymentComponent } from './payment/payment.component';
import { LibraryComponent } from './library/library.component';

const routes: Routes = [
  { path: '', component: StorefrontComponent},
  { path: 'public/browse-games', component: BrowseGamesComponent},
  { path: 'public/games/:id', component: GameDetailsComponent},
  { path: 'public/cart', component: CartComponent},
  { path: 'public/checkout', component: BeginCheckoutComponent},
  { path: 'public/payment', component: PaymentComponent},
  { path: 'public/payment/success', component: PaymentComponent, data: { mode: 'success' }},
  { path: 'public/library', component: LibraryComponent},
  { path: '**', redirectTo: '' }, //if user goes to any url that doesnt exist, send them back to '/'
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {}
