import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {StorefrontComponent} from './storefront/storefront.component';
import {SharedModule} from '../shared/shared-module';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { GameDetailsComponent } from './game-details/game-details.component';
import { CartComponent } from './cart/cart.component';
import { BeginCheckoutComponent } from './begin-checkout/begin-checkout.component';
import { PaymentComponent } from './payment/payment.component';
import { LibraryComponent } from './library/library.component';
import { ProfileComponent } from './user-profile/profile/profile.component';
import { UploadProfilePictureDialogModule } from './user-profile/profile/upload-profile-picture-dialog/upload-profile-picture-dialog.module';
import { CommonModule, DatePipe } from '@angular/common';
import { UserSecurityComponent } from './user-profile/user-security/user-security.component';


@NgModule({
  declarations: [
    StorefrontComponent,
    BrowseGamesComponent,
    GameDetailsComponent,
    CartComponent,
    BeginCheckoutComponent,
    PaymentComponent,
    LibraryComponent,
    ProfileComponent,
    UserSecurityComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    UploadProfilePictureDialogModule,
    PublicRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    DatePipe,
    MatIconModule,
  ]
})
export class PublicModule { }
