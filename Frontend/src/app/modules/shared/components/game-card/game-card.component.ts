import { Component, Input } from '@angular/core';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';

@Component({
  selector: 'app-game-card',
  standalone: false,
  templateUrl: './game-card.component.html',
  styleUrl: './game-card.component.scss',
})
export class GameCardComponent {
  @Input({ required: true }) game!: StorefrontGameDto;

  getGameImage(): string {
    return this.game.coverImageURL ?? this.game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }
}
