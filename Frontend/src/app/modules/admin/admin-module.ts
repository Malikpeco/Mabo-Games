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

  ],
  imports: [
    AdminRoutingModule,
    SharedModule,
  ]
})
export class AdminModule { }
