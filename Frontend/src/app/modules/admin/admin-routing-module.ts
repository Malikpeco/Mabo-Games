import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { GameFormComponent } from './games/game-form/game-form.component';
import { AdminGamesComponent } from './games/admin-games/admin-games.component';
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
        component: GameFormComponent,
      },
      {
        path: 'games/:id/edit',
        component: GameFormComponent,
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
