import {NgModule} from '@angular/core';
import {SharedModule} from '../shared/shared-module';
import { AdminRoutingModule } from './admin-routing-module';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminGamesComponent } from './games/admin-games/admin-games.component';
import { AdminGamesAddComponent } from './games/admin-games-add/admin-games-add.component';
import { AdminGenresComponent } from './genres/admin-genres/admin-genres.component';
import { AdminPublishersComponent } from './publishers/admin-publishers/admin-publishers.component';
import { AdminOrdersComponent } from './orders/admin-orders/admin-orders.component';
import { AdminOrderDetailsComponent } from './orders/admin-order-details/admin-order-details.component';
import { AdminGamesEditComponent } from './games/admin-games-edit/admin-games-edit.component';
import { CreatePublisherDialogComponent } from './games/admin-shared/create-publisher-dialog/create-publisher-dialog.component';
import { PublisherDropdownComponent } from './games/admin-shared/publisher-dropdown/publisher-dropdown.component';
import { GenreDropdownComponent } from './games/admin-shared/genre-dropdown/genre-dropdown.component';
import { CountryDropdownComponent } from './games/admin-shared/country-dropdown/country-dropdown.component';



@NgModule({
  declarations: [ 
    AdminPanelComponent, 
    AdminGamesComponent, 
    AdminGamesAddComponent, 
    AdminGenresComponent, 
    AdminPublishersComponent, 
    AdminOrdersComponent, 
    AdminOrderDetailsComponent, 
    AdminGamesEditComponent,
    CreatePublisherDialogComponent,
    PublisherDropdownComponent,
    GenreDropdownComponent,
    CountryDropdownComponent,

  ],
  imports: [
    AdminRoutingModule,
    SharedModule,
  ]
})
export class AdminModule { }
