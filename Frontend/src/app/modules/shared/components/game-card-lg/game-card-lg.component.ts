import { Component, Input } from '@angular/core';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';

@Component({
  selector: 'app-game-card-lg',
  standalone: false,
  templateUrl: './game-card-lg.component.html',
  styleUrl: './game-card-lg.component.scss',
})
export class GameCardLgComponent {
  @Input({ required: true }) game!: StorefrontGameDto;

  getGameImage(): string {
    return this.game.coverImageURL ?? this.game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }

  get isFree(): boolean {
    return this.game.price <= 0;
  }

  get priceLabel(): string {
    return this.isFree ? 'Free' : `${this.game.price.toFixed(2)} €`;
  }
}
