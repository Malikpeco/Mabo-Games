import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StorefrontComponent } from '../public/storefront/storefront.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminGamesAddComponent } from './games/admin-games-add/admin-games-add.component';
import { AdminGamesComponent } from './games/admin-games/admin-games.component';
import { AdminGamesEditComponent } from './games/admin-games-edit/admin-games-edit.component';
import { AdminPublishersComponent } from './publishers/admin-publishers/admin-publishers.component';
import { AdminGenresComponent } from './genres/admin-genres/admin-genres.component';
import { AdminOrderDetailsComponent } from './orders/admin-order-details/admin-order-details.component';
import { AdminOrdersComponent } from './orders/admin-orders/admin-orders.component';


const routes: Routes = [
  {
    path: '',
    component: AdminPanelComponent,
    children: [

      // Games Panel
      {
        path: 'games',
        component: AdminGamesComponent,
      },
      {
        path: 'games/add',
        component: AdminGamesAddComponent,
      },
      {
        path: 'games/:id/edit',
        component: AdminGamesEditComponent,
      },



      // Publishers panel
      {
        path: 'publishers',
        component: AdminPublishersComponent,
      },

      // GENRES
      {
        path: 'genres',
        component: AdminGenresComponent,
      },

      // Ordrers panel
      {
        path: 'orders',
        component: AdminOrdersComponent,
      },
      {
        path: 'orders/:id',
        component: AdminOrderDetailsComponent,
      },

      // Default route back to games
      {
        path: '',
        redirectTo: 'games',
        pathMatch: 'full',
      },


    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule { }
