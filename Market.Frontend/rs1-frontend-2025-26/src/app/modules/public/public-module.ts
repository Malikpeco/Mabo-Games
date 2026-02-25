import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {StorefrontComponent} from './storefront/storefront.component';
import {SharedModule} from '../shared/shared-module';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { FormsModule } from '@angular/forms';
import { GameDetailsComponent } from './game-details/game-details.component';
import { MaboNavbarComponent } from './mabo-navbar/mabo-navbar.component';
import { MaboFooterComponent } from './mabo-footer/mabo-footer.component';


@NgModule({
  declarations: [
    StorefrontComponent,
    BrowseGamesComponent,
    GameDetailsComponent,
    MaboNavbarComponent,
    MaboFooterComponent,
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
    FormsModule,
  ]
})
export class PublicModule { }
