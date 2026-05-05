import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GenreDto } from '../../../../api-services/genres/genres-api.models';

@Component({
  selector: 'app-genre-list-item',
  standalone: false,
  templateUrl: './genre-list-item.component.html',
  styleUrl: './genre-list-item.component.scss',
})
export class GenreListItemComponent {
  @Input({ required: true }) genre!: GenreDto;

  @Output() editGenre = new EventEmitter<GenreDto>();
  @Output() deleteGenre = new EventEmitter<GenreDto>();

  get gameCount(): number {
    return this.genre.gameCount ?? 0;
  }

  get gameCountLabel(): string {
    const count = this.gameCount;
    return count === 1 ? '1 game' : `${count} games`;
  }

  onEdit(): void {
    this.editGenre.emit(this.genre);
  }

  onDelete(): void {
    this.deleteGenre.emit(this.genre);
  }
}