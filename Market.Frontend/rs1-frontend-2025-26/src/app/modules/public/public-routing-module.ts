import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StorefrontComponent } from './storefront/storefront.component';
import { BrowseGamesComponent } from './browse-games/browse-games.component';
import { GameDetailsComponent } from './game-details/game-details.component';

const routes: Routes = [
  { path: '', component: StorefrontComponent},
  { path: 'public/browse-games', component: BrowseGamesComponent},
  { path: 'public/games/:id', component: GameDetailsComponent},
  { path: '**', redirectTo: '' }, //if user goes to any url that doesnt exist, send them back to '/'
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {}
