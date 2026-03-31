import { Component, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { GenreDto } from '../../../../api-services/genres/genres-api.models';
import { GenresApiService } from '../../../../api-services/genres/genres-api.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import { CreateGenreDialogComponent, CreateGenreDialogResult } from '../create-genre-dialog/create-genre-dialog.component';

@Component({
  selector: 'app-admin-genres',
  standalone: false,
  templateUrl: './admin-genres.component.html',
  styleUrl: './admin-genres.component.scss',
})
export class AdminGenresComponent implements OnInit {
  private genresApi = inject(GenresApiService);
  private dialog = inject(DialogHelperService);
  private matDialog = inject(MatDialog);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private requestSeq = 0;

  genres: GenreDto[] = [];
  searchTerm = '';
  sort = 'nameAsc';
  readonly sortOptions: Array<{ value: string; label: string }> = [
    { value: 'nameAsc', label: 'Name: A-Z' },
    { value: 'nameDesc', label: 'Name: Z-A' },
    { value: 'mostGames', label: 'Most Games' },
  ];
  isLoading = false;
  page = 1;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  totalCount = 0;

  ngOnInit(): void {
    this.loadGenres();
  }

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get hasSearchTerm(): boolean {
    return this.searchTerm.trim().length > 0;
  }

  get filteredGenres(): GenreDto[] {
    if (this.sort === 'nameDesc') {
      return [...this.genres].sort((a, b) => b.name.localeCompare(a.name));
    }

    if (this.sort === 'mostGames') {
      return [...this.genres].sort((a, b) => {
        const gameDelta = (b.gameCount ?? 0) - (a.gameCount ?? 0);
        if (gameDelta !== 0) {
          return gameDelta;
        }

        return a.name.localeCompare(b.name);
      });
    }

    return [...this.genres].sort((a, b) => a.name.localeCompare(b.name));
  }

  onSortChange(sort: string): void {
    if (!sort || sort === this.sort) {
      return;
    }

    this.sort = sort;
  }

  onSearchChange(term: string): void {
    this.searchTerm = term ?? '';
    this.page = 1;

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    this.searchDebounceTimer = setTimeout(() => {
      this.loadGenres();
    }, 250);
  }

  goToPage(p: number): void {
    if (p < 1 || p > this.totalPages || p === this.page) {
      return;
    }

    this.page = p;
    this.loadGenres();
  }

  onPageSizeChange(size: number | string): void {
    const parsedSize = Number(size);
    if (!Number.isFinite(parsedSize) || parsedSize <= 0 || parsedSize === this.pageSize) {
      return;
    }

    this.pageSize = parsedSize;
    this.page = 1;
    this.loadGenres();
  }

  onAddGenre(): void {
    const dialogRef = this.matDialog.open(CreateGenreDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: { initialTitle: this.searchTerm.trim(), mode: 'create' },
    });

    dialogRef.afterClosed().subscribe((result: CreateGenreDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.genresApi.create(result.title).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Genre created',
            `Genre "${result.title}" was created successfully.`,
            undefined,
            'check_circle'
          );

          this.searchTerm = '';
          this.page = 1;
          this.loadGenres();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not create genre. Please try again.';

          this.dialog.showError('Create failed', message, undefined, 'error');
        },
      });
    });
  }

  onEditGenre(genre: GenreDto): void {
    const dialogRef = this.matDialog.open(CreateGenreDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: { initialTitle: genre.name, mode: 'edit' },
    });

    dialogRef.afterClosed().subscribe((result: CreateGenreDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.genresApi.update(genre.id, { name: result.title }).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Genre updated',
            `Genre "${result.title}" was updated successfully.`,
            undefined,
            'check_circle'
          );

          this.loadGenres();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not update genre. Please try again.';

          this.dialog.showError('Update failed', message, undefined, 'error');
        },
      });
    });
  }

  onDeleteGenre(genre: GenreDto): void {
    const assignedGamesCount = genre.gameCount ?? 0;
    if (assignedGamesCount > 0) {
      const gameLabel = assignedGamesCount === 1 ? 'game' : 'games';
      this.dialog.showWarning(
        'Delete blocked',
        `Genre "${genre.name}" cannot be deleted because it has ${assignedGamesCount} ${gameLabel}.`,
        undefined,
        'warning'
      );
      return;
    }

    this.dialog.confirmDelete(genre.name).subscribe((response) => {
      if (response?.button !== DialogButton.DELETE) {
        return;
      }

      this.genresApi.delete(genre.id).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Genre deleted',
            `Genre "${genre.name}" was deleted successfully.`,
            undefined,
            'check_circle'
          );

          const wasLastItemOnPage = this.genres.length === 1;
          if (wasLastItemOnPage && this.page > 1) {
            this.page = this.page - 1;
          }

          this.loadGenres();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not delete genre. Please try again.';

          this.dialog.showError('Delete failed', message, undefined, 'error');
        },
      });
    });
  }

  private loadGenres(): void {
    const requestId = ++this.requestSeq;
    this.isLoading = true;

    this.genresApi.list({
      paging: { page: this.page, pageSize: this.pageSize },
      search: this.searchTerm.trim() || null,
    }).subscribe({
      next: (res) => {
        if (requestId !== this.requestSeq) {
          return;
        }

        const nextTotalCount = res.total ?? 0;
        const nextTotalPages = Math.max(1, Math.ceil(nextTotalCount / this.pageSize));
        if (this.page > nextTotalPages) {
          this.page = nextTotalPages;
          this.loadGenres();
          return;
        }

        this.genres = res.items ?? [];
        this.totalCount = nextTotalCount;
        this.isLoading = false;
      },
      error: () => {
        if (requestId !== this.requestSeq) {
          return;
        }

        this.genres = [];
        this.totalCount = 0;
        this.isLoading = false;
      },
    });
  }

}
