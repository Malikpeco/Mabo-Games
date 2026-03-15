import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';

@Component({
  selector: 'app-game-list',
  standalone: false,
  templateUrl: './game-list.component.html',
  styleUrl: './game-list.component.scss',
})
export class GameListComponent {
  @Input({ required: true }) game!: StorefrontGameDto;

  @Output() editGame = new EventEmitter<StorefrontGameDto>();
  @Output() deleteGame = new EventEmitter<StorefrontGameDto>();

  getGameImage(): string {
    return this.game.coverImageURL ?? this.game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }

  get priceLabel(): string {
    return this.game.price <= 0
      ? 'Free'
      : `${this.game.price.toFixed(2)} EUR`;
  }

  onEdit(): void {
    this.editGame.emit(this.game);
  }

  onDelete(): void {
    this.deleteGame.emit(this.game);
  }
}
