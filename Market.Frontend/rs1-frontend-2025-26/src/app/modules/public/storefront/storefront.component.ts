import { Component, inject } from '@angular/core';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';

@Component({
  selector: 'app-storefront',
  standalone: false,
  templateUrl: './storefront.component.html',
  styleUrl: './storefront.component.scss',
})
export class StorefrontComponent {
  private gamesApi = inject(GamesApiService);
  
  currentYear: string = "2025";

  newestGames: StorefrontGameDto[] = [];

  ngOnInit(): void {
    this.gamesApi.storefront({
      paging:{page:1, pageSize: 5},
    }).subscribe(res=>{
      this.newestGames = res.items;
    });
  }

  getGameImage(game: StorefrontGameDto): string{
    return game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }

}
