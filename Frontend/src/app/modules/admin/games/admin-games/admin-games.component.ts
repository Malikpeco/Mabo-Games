import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { StorefrontGameDto } from '../../../../api-services/games/games-api.models';
import { GamesApiService } from '../../../../api-services/games/games-api.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';

@Component({
  selector: 'app-admin-games',
  standalone: false,
  templateUrl: './admin-games.component.html',
  styleUrl: './admin-games.component.scss',
})
export class AdminGamesComponent implements OnInit, OnDestroy {
  private gamesApi = inject(GamesApiService);
  private router = inject(Router);
  private dialog = inject(DialogHelperService);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private requestSeq = 0;

  games: StorefrontGameDto[] = [];
  searchTerm = '';
  sort = 'recent';
  readonly sortOptions: Array<{ value: string; label: string }> = [
    { value: 'nameAsc', label: 'Name: A-Z' },
    { value: 'nameDesc', label: 'Name: Z-A' },
    { value: 'priceAsc', label: 'Price: Low to High' },
    { value: 'priceDesc', label: 'Price: High to Low' },
    { value: 'recent', label: 'Newest First' },
  ];
  isLoading = false;
  page = 1;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  totalCount = 0;

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get hasSearchTerm(): boolean {
    return this.searchTerm.trim().length > 0;
  }

  get filteredGames(): StorefrontGameDto[] {
    return this.games;
  }

  ngOnInit(): void {
    this.loadGames();
  }

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }
  }

  loadGames(): void {
    const requestId = ++this.requestSeq;
    this.isLoading = true;

    this.gamesApi
      .storefront({
        paging: { page: this.page, pageSize: this.pageSize },
        sort: this.sort,
        search: this.searchTerm.trim() || null
      })
      .subscribe({
        next: (res) => {
          if (requestId !== this.requestSeq) {
            return;
          }

          const nextTotalCount = res.total ?? 0;

          const nextTotalPages = Math.max(1, Math.ceil(nextTotalCount / this.pageSize));
          if (this.page > nextTotalPages) {
            this.page = nextTotalPages;
            this.loadGames();
            return;
          }

          this.games = res.items ?? [];
          this.totalCount = nextTotalCount;
          this.isLoading = false;
        },
        error: () => {
          if (requestId !== this.requestSeq) {
            return;
          }

          this.games = [];
          this.totalCount = 0;
          this.isLoading = false;
        },
      });
  }

  goToPage(p: number): void {
    if (p < 1 || p > this.totalPages || p === this.page) return;
    this.page = p;
    this.loadGames();
  }

  onPageSizeChange(size: number | string): void {
    const parsedSize = Number(size);
    if (!Number.isFinite(parsedSize) || parsedSize <= 0) return;
    if (parsedSize === this.pageSize) return;

    this.pageSize = parsedSize;
    this.page = 1;
    this.loadGames();
  }

  onSortChange(sort: string): void {
    if (!sort || sort === this.sort) return;
    this.sort = sort;
    this.page = 1;
    this.loadGames();
  }

  onSearchChange(term: string): void {
    this.searchTerm = term ?? '';
    this.page = 1;

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    this.searchDebounceTimer = setTimeout(() => {
      this.loadGames();
    }, 250);
  }

  onEditGame(game: StorefrontGameDto): void {
    this.router.navigate(['/admin/games', game.id, 'edit']);
  }

  onDeleteGame(game: StorefrontGameDto): void {
    this.dialog.confirmDelete(game.name).subscribe((response) => {
      if (response?.button !== DialogButton.DELETE) {
        return;
      }

      this.gamesApi.delete(game.id).subscribe({
        next: () => {
          this.dialog.showSuccess('Game deleted', `Game "${game.name}" was deleted successfully.`, undefined, 'check_circle');

          const wasLastItemOnPage = this.games.length === 1;
          if (wasLastItemOnPage && this.page > 1) {
            this.page = this.page - 1;
          }

          this.loadGames();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not delete game. Please try again.';

          this.dialog.showError('Delete failed', message, undefined, 'error');
        },
      });
    });
  }

}
