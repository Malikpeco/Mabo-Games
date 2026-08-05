import { Component, Input } from '@angular/core';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';

@Component({
  selector: 'app-game-card-sm',
  standalone: false,
  templateUrl: './game-card-sm.component.html',
  styleUrl: './game-card-sm.component.scss',
})
export class GameCardSmComponent {
  @Input({ required: true }) game!: StorefrontGameDto;

  getGameImage(): string {
    return this.game.screenshots?.[0]?.imageURL ?? this.game.coverImageURL ?? '/carousel-placeholder-image.png';
  }

  get isFree(): boolean {
    return this.game.price <= 0;
  }

  get priceLabel(): string {
    return this.isFree ? 'Free' : `${this.game.price.toFixed(2)} €`;
  }
}
