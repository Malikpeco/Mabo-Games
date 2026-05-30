import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { GenreDto } from '../../../api-services/genres/genres-api.models';
import { GenresApiService } from '../../../api-services/genres/genres-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
import { SupabaseApiService } from '../../../api-services/supabase/supabase-api.service';
import { UserGamesApiService } from '../../../api-services/user-games/user-games-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import {FavouritesApiService } from '../../../api-services/favourites/favourites-api.service';
import { ListUserGamesQueryDto, ListUserGamesRequest } from '../../../api-services/user-games/user-games-api.models';
import { ListFavouritesQueryRequest } from '../../../api-services/favourites/favourites-api.models';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-library',
  standalone: false,
  templateUrl: './library.component.html',
  styleUrl: './library.component.scss',
})
export class LibraryComponent
  extends BaseListPagedComponent<ListUserGamesQueryDto, ListUserGamesRequest>
  implements OnInit {
  private userGamesApi = inject(UserGamesApiService);
  private genresApi = inject(GenresApiService);
  private supabaseApi = inject(SupabaseApiService);
  private favouritesApi = inject(FavouritesApiService);
  private currentUserService = inject(CurrentUserService);
  private router = inject(Router);

  isAuthenticated = this.currentUserService.isAuthenticated;

  search = '';
  genres: GenreDto[] = [];
  selectedGenreIds = new Set<number>();

  filteredGames: ListUserGamesQueryDto[] = [];
  favouriteGameIds = new Set<number>();
  favouriteGames: ListUserGamesQueryDto[] = [];
  downloadingGameId: number | null = null;
  togglingFavouriteGameId: number | null = null;

  constructor() {
    super();
    this.request = new ListUserGamesRequest();
    this.request.paging.page = 1;
    this.request.paging.pageSize = 1000;
  }


  ngOnInit(): void {
    if (!this.isAuthenticated()) {
      return;
    }

    this.genresApi.list({ paging: { page: 1, pageSize: 1000 } }).subscribe(res => {
      this.genres = res.items ?? [];
    });

    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.userGamesApi.listUserGames(this.request).subscribe({
      next: (res) => {
        this.handlePageResult(res);
        this.loadFavourites();
        this.applyFilters();
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load your library.');
        console.error('Load library error:', err);
      }
    });
  }

  private loadFavourites(): void {
    const request = new ListFavouritesQueryRequest();
    request.paging.page = 1;
    request.paging.pageSize = 1000;

    this.favouritesApi.listFavouritesQuery(request).subscribe({
      next: res => {
        this.favouriteGameIds = new Set((res.items ?? []).map(game => game.id));
        this.updatePinnedFavourites();
      },
      error: () => {
        this.favouriteGameIds = new Set<number>();
        this.updatePinnedFavourites();
      }
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

  getPriceLabel(game: StorefrontGameDto): string {
    return game.price <= 0 ? 'Free' : `${game.price.toFixed(2)} EUR`;
  }

  goToFavourites(): void {
    this.router.navigate(['/public/favourites']);
  }

  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }

  openGameDetails(gameId: number): void {
    this.router.navigate(['/public/games', gameId]);
  }

  downloadGame(game: ListUserGamesQueryDto, event?: MouseEvent): void {
    event?.preventDefault();
    event?.stopPropagation();

    if (this.downloadingGameId === game.gameId) {
      return;
    }

    this.downloadingGameId = game.gameId;

    this.supabaseApi.getGameDownloadUrl(game.gameId).subscribe({
      next: (downloadUrl) => {
        const link = document.createElement('a');
        link.href = downloadUrl;
        link.target = '_blank';
        link.rel = 'noopener noreferrer';
        document.body.appendChild(link);
        link.click();
        link.remove();
      },
      error: (err) => {
        console.error('Failed to get game download URL:', err);
        this.downloadingGameId = null;
      },
      complete: () => {
        this.downloadingGameId = null;
      },
    });
  }

  toggleFavourite(game: ListUserGamesQueryDto, event?: MouseEvent): void {
    event?.preventDefault();
    event?.stopPropagation();

    if (this.togglingFavouriteGameId === game.gameId) {
      return;
    }

    this.togglingFavouriteGameId = game.gameId;

    const request = this.favouriteGameIds.has(game.gameId)
      ? this.favouritesApi.removeFromFavourites(game.gameId)
      : this.favouritesApi.addToFavourites(game.gameId);

    request.subscribe({
      next: () => {
        if (this.favouriteGameIds.has(game.gameId)) {
          this.favouriteGameIds.delete(game.gameId);
        } else {
          this.favouriteGameIds.add(game.gameId);
        }

        this.updatePinnedFavourites();
      },
      error: (err) => {
        console.error('Failed to update favourites:', err);
        this.togglingFavouriteGameId = null;
      },
      complete: () => {
        this.togglingFavouriteGameId = null;
      },
    });
  }

  retry(): void {
    this.loadPagedData();
  }

  trackByGameId(_: number, game: ListUserGamesQueryDto): number {
    return game.gameId;
  }

  private applyFilters(): void {
    const searchText = this.search.trim().toLowerCase();
    const hasGenreFilter = this.selectedGenreIds.size > 0;

    this.filteredGames = this.items.filter(game => {
      const matchesSearch =
        !searchText ||
        game.game.name.toLowerCase().includes(searchText) ||
        game.game.publisherName.toLowerCase().includes(searchText);

      const matchesGenre =
        !hasGenreFilter ||
        (game.game.genres ?? []).some(genre => this.selectedGenreIds.has(genre.id));

      return matchesSearch && matchesGenre;
    });

    this.updatePinnedFavourites();
  }

  private updatePinnedFavourites(): void {
    this.favouriteGames = this.filteredGames.filter(usergame => this.favouriteGameIds.has(usergame.gameId));
  }
}
