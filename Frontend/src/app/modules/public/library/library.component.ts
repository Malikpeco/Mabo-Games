import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { GenreDto } from '../../../api-services/genres/genres-api.models';
import { GenresApiService } from '../../../api-services/genres/genres-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
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

  constructor() {
    super();
    this.request = new ListUserGamesRequest();
  }


  ngOnInit(): void {
    if (!this.isAuthenticated()) {
      return;
    }

    this.genresApi.list().subscribe(res => {
      this.genres = res ?? [];
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
    this.request.paging.page = 1;
    this.loadPagedData();
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

    this.request.paging.page = 1;
    this.loadPagedData();
  }

  clearGenres(): void {
    this.selectedGenreIds.clear();
    this.request.paging.page = 1;
    this.loadPagedData();
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

  retry(): void {
    this.loadPagedData();
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
