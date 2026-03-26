import {NgModule} from '@angular/core';
import {SharedModule} from '../shared/shared-module';
import { AdminRoutingModule } from './admin-routing-module';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';

import { AdminGenresComponent } from './genres/admin-genres/admin-genres.component';
import { AdminPublishersComponent } from './publishers/admin-publishers/admin-publishers.component';
import { AdminOrdersComponent } from './orders/admin-orders/admin-orders.component';
import { AdminOrderDetailsComponent } from './orders/admin-order-details/admin-order-details.component';
import { GameFormComponent } from './games/game-form/game-form.component';
import { CreatePublisherDialogComponent } from './games/admin-shared/create-publisher-dialog/create-publisher-dialog.component';
import { PublisherDropdownComponent } from './games/admin-shared/publisher-dropdown/publisher-dropdown.component';
import { GenreDropdownComponent } from './games/admin-shared/genre-dropdown/genre-dropdown.component';
import { CountryDropdownComponent } from './games/admin-shared/country-dropdown/country-dropdown.component';
import { IgdbSearchComponent } from './games/admin-shared/igdb-search/igdb-search.component';
import { AdminGamesComponent } from './games/admin-games/admin-games.component';
import { PublisherListItemComponent } from './publishers/publisher-list-item/publisher-list-item.component';




@NgModule({
  declarations: [ 
    AdminPanelComponent, 
    AdminGamesComponent, 
    AdminGenresComponent, 
    AdminPublishersComponent, 
    AdminOrdersComponent, 
    AdminOrderDetailsComponent, 
    GameFormComponent,
    CreatePublisherDialogComponent,
    PublisherDropdownComponent,
    GenreDropdownComponent,
    CountryDropdownComponent,
    IgdbSearchComponent,
    PublisherListItemComponent,

  ],
  imports: [
    AdminRoutingModule,
    SharedModule,
  ]
})
export class AdminModule { }
