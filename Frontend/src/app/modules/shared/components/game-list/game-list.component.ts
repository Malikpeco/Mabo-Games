import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';
import { APP_LOCALE } from '../../../../core/constants/locale';

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

  get releaseDateLabel(): string {
    const raw = this.game.releaseDate;
    if (!raw) {
      return 'N/A';
    }

    const parsed = new Date(raw);
    if (Number.isNaN(parsed.getTime())) {
      return 'N/A';
    }

    return new Intl.DateTimeFormat(APP_LOCALE, {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
    }).format(parsed);
  }

  onEdit(): void {
    this.editGame.emit(this.game);
  }

  onDelete(): void {
    this.deleteGame.emit(this.game);
  }
}
