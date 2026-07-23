import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ListUserGamesQueryDto } from '../../../../api-services/user-games/user-games-api.models';

@Component({
  selector: 'app-library-game-card',
  standalone: false,
  templateUrl: './library-game-card.component.html',
  styleUrl: './library-game-card.component.scss',
})
export class LibraryGameCardComponent {
  @Input({ required: true }) userGame!: ListUserGamesQueryDto;
  @Input() isFavourite = false;
  @Input() isDownloading = false;

  @Output() openGame = new EventEmitter<number>();
  @Output() toggleFavourite = new EventEmitter<ListUserGamesQueryDto>();
  @Output() downloadGame = new EventEmitter<ListUserGamesQueryDto>();

  getGameImage(): string {
    return this.userGame.game.coverImageURL ?? this.userGame.game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }

  onOpenGame(): void {
    this.openGame.emit(this.userGame.gameId);
  }

  onFavouriteClick(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.toggleFavourite.emit(this.userGame);
  }

  onDownloadClick(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.downloadGame.emit(this.userGame);
  }
}
