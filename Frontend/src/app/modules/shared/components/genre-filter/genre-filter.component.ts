import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GenreDto } from '../../../../api-services/genres/genres-api.models';

@Component({
  selector: 'app-genre-filter',
  standalone: false,
  templateUrl: './genre-filter.component.html',
  styleUrl: './genre-filter.component.scss',
})
export class GenreFilterComponent {
  @Input() title = 'Genres';
  @Input() genres: GenreDto[] = [];
  @Input() selectedGenreIds = new Set<number>();

  @Output() toggleGenre = new EventEmitter<number>();
  @Output() clearGenres = new EventEmitter<void>();

  isSelected(id: number): boolean {
    return this.selectedGenreIds.has(id);
  }

  onToggleGenre(id: number): void {
    this.toggleGenre.emit(id);
  }

  onClearGenres(): void {
    this.clearGenres.emit();
  }
}
