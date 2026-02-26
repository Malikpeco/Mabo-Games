import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { GenreDto } from '../../../api-services/genres/genres-api.models';
import { GenresApiService } from '../../../api-services/genres/genres-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
import { UserGamesApiService } from '../../../api-services/user-games/user-games-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-library',
  standalone: false,
  templateUrl: './library.component.html',
  styleUrl: './library.component.scss',
})
export class LibraryComponent implements OnInit {
  private userGamesApi = inject(UserGamesApiService);
  private genresApi = inject(GenresApiService);
  private currentUserService = inject(CurrentUserService);
  private router = inject(Router);

  isAuthenticated = this.currentUserService.isAuthenticated;

  isLoading = false;
  errorMessage = '';

  search = '';
  genres: GenreDto[] = [];
  selectedGenreIds = new Set<number>();

  games: StorefrontGameDto[] = [];
  filteredGames: StorefrontGameDto[] = [];

  ngOnInit(): void {
    if (!this.isAuthenticated()) {
      return;
    }

    this.genresApi.list().subscribe(res => {
      this.genres = res.items ?? [];
    });

    this.loadLibrary();
  }

  private loadLibrary(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.userGamesApi.listUserGames().subscribe({
      next: res => {
        this.games = res.items ?? [];
        this.applyFilters();
        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = err?.error?.message ?? 'Failed to load your library.';
        this.isLoading = false;
      }
    });
  }

  private applyFilters(): void {
    const searchText = this.search.trim().toLowerCase();
    const hasGenreFilter = this.selectedGenreIds.size > 0;

    this.filteredGames = this.games.filter(game => {
      const matchesSearch =
        !searchText ||
        game.name.toLowerCase().includes(searchText) ||
        game.publisherName.toLowerCase().includes(searchText);

      const matchesGenre =
        !hasGenreFilter ||
        (game.genres ?? []).some(genre => this.selectedGenreIds.has(genre.id));

      return matchesSearch && matchesGenre;
    });
  }

  onSearchChanged(): void {
    this.applyFilters();
  }

  isGenreSelected(id: number): boolean {
    return this.selectedGenreIds.has(id);
  }

  toggleGenre(id: number): void {
    if (this.selectedGenreIds.has(id)) {
      this.selectedGenreIds.delete(id);
    } else {
      this.selectedGenreIds.add(id);
    }

    this.applyFilters();
  }

  clearGenres(): void {
    this.selectedGenreIds.clear();
    this.applyFilters();
  }

  getGameImage(game: StorefrontGameDto): string {
    return game.coverImageURL ?? game.screenshots[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }

  goToFavourites(): void {
    this.router.navigate(['/public/favourites']);
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }

  retry(): void {
    this.loadLibrary();
  }
}
