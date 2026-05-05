import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ListPublisherDto } from '../../../../api-services/publishers/publishers-api.models';
import { PublishersApiService } from '../../../../api-services/publishers/publishers-api.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import { CreatePublisherDialogComponent, CreatePublisherDialogResult } from '../../games/admin-shared/create-publisher-dialog/create-publisher-dialog.component';

@Component({
  selector: 'app-admin-publishers',
  standalone: false,
  templateUrl: './admin-publishers.component.html',
  styleUrl: './admin-publishers.component.scss',
})
export class AdminPublishersComponent implements OnInit, OnDestroy {
  private publishersApi = inject(PublishersApiService);
  private dialog = inject(DialogHelperService);
  private matDialog = inject(MatDialog);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private requestSeq = 0;

  publishers: ListPublisherDto[] = [];
  searchTerm = '';
  sort = 'recent';
  readonly sortOptions: Array<{ value: string; label: string }> = [
    { value: 'nameAsc', label: 'Name: A-Z' },
    { value: 'nameDesc', label: 'Name: Z-A' },
    { value: 'mostGames', label: 'Most Games' },
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

  get filteredPublishers(): ListPublisherDto[] {
    if (this.sort === 'nameAsc') {
      return [...this.publishers].sort((a, b) => a.name.localeCompare(b.name));
    }

    if (this.sort === 'nameDesc') {
      return [...this.publishers].sort((a, b) => b.name.localeCompare(a.name));
    }

    if (this.sort === 'mostGames') {
      return [...this.publishers].sort((a, b) => {
        const gamesDelta = (b.games?.length ?? 0) - (a.games?.length ?? 0);
        if (gamesDelta !== 0) {
          return gamesDelta;
        }

        return a.name.localeCompare(b.name);
      });
    }

    return this.publishers;
  }

  ngOnInit(): void {
    this.loadPublishers();
  }

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }
  }

  loadPublishers(): void {
    const requestId = ++this.requestSeq;
    this.isLoading = true;

    this.publishersApi
      .list({
        paging: { page: this.page, pageSize: this.pageSize },
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
            this.loadPublishers();
            return;
          }

          this.publishers = res.items ?? [];
          this.totalCount = nextTotalCount;
          this.isLoading = false;
        },
        error: () => {
          if (requestId !== this.requestSeq) {
            return;
          }

          this.publishers = [];
          this.totalCount = 0;
          this.isLoading = false;
        },
      });
  }

  goToPage(p: number): void {
    if (p < 1 || p > this.totalPages || p === this.page) return;
    this.page = p;
    this.loadPublishers();
  }

  onPageSizeChange(size: number | string): void {
    const parsedSize = Number(size);
    if (!Number.isFinite(parsedSize) || parsedSize <= 0) return;
    if (parsedSize === this.pageSize) return;

    this.pageSize = parsedSize;
    this.page = 1;
    this.loadPublishers();
  }

  onSortChange(sort: string): void {
    if (!sort || sort === this.sort) return;
    this.sort = sort;
  }

  onSearchChange(term: string): void {
    this.searchTerm = term ?? '';
    this.page = 1;

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    this.searchDebounceTimer = setTimeout(() => {
      this.loadPublishers();
    }, 250);
  }

  onAddPublisher(): void {
    const dialogRef = this.matDialog.open(CreatePublisherDialogComponent, {
      width: '760px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: { initialTitle: '' },
    });

    dialogRef.afterClosed().subscribe((result: CreatePublisherDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.publishersApi.create({
        name: result.title,
        countryId: result.countryId,
      }).subscribe({
        next: () => {
          this.dialog.showSuccess('Publisher created', `Publisher "${result.title}" was created successfully.`, undefined, 'check_circle');
          this.page = 1;
          this.loadPublishers();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not create publisher. Please try again.';

          this.dialog.showError('Create failed', message, undefined, 'error');
        },
      });
    });
  }

  onEditPublisher(publisher: ListPublisherDto): void {
    this.publishersApi.getById(publisher.id).subscribe({
      next: (publisherDetails) => {
        const dialogRef = this.matDialog.open(CreatePublisherDialogComponent, {
          width: '760px',
          maxWidth: 'calc(100vw - 24px)',
          disableClose: false,
          panelClass: ['custom-dialog-container', 'site-dialog-panel'],
          backdropClass: 'site-dialog-backdrop',
          data: {
            initialTitle: publisherDetails.name,
            initialCountryId: publisherDetails.country?.id,
            initialCountryName: publisherDetails.country?.name,
            mode: 'edit',
          },
        });

        dialogRef.afterClosed().subscribe((result: CreatePublisherDialogResult | null | undefined) => {
          if (!result) {
            return;
          }

          this.publishersApi.update(publisher.id, {
            name: result.title,
            countryId: result.countryId,
          }).subscribe({
            next: () => {
              this.dialog.showSuccess('Publisher updated', `Publisher "${result.title}" was updated successfully.`, undefined, 'check_circle');
              this.loadPublishers();
            },
            error: (error: HttpErrorResponse) => {
              const message =
                error.error?.message ||
                error.error?.title ||
                'Could not update publisher. Please try again.';

              this.dialog.showError('Update failed', message, undefined, 'error');
            },
          });
        });
      },
      error: (error: HttpErrorResponse) => {
        const message =
          error.error?.message ||
          error.error?.title ||
          'Could not load publisher details. Please try again.';

        this.dialog.showError('Load failed', message, undefined, 'error');
      },
    });
  }

  onDeletePublisher(publisher: ListPublisherDto): void {
    const assignedGamesCount = publisher.games?.length ?? 0;
    if (assignedGamesCount > 0) {
      const gameLabel = assignedGamesCount === 1 ? 'game' : 'games';
      this.dialog.showWarning(
        'Delete blocked',
        `Publisher "${publisher.name}" cannot be deleted because it has ${assignedGamesCount} ${gameLabel}.`,
        undefined,
        'warning'
      );
      return;
    }

    this.dialog.confirmDelete(publisher.name).subscribe((response) => {
      if (response?.button !== DialogButton.DELETE) {
        return;
      }

      this.publishersApi.delete(publisher.id).subscribe({
        next: () => {
          this.dialog.showSuccess('Publisher deleted', `Publisher "${publisher.name}" was deleted successfully.`, undefined, 'check_circle');

          const wasLastItemOnPage = this.publishers.length === 1;
          if (wasLastItemOnPage && this.page > 1) {
            this.page = this.page - 1;
          }

          this.loadPublishers();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not delete publisher. Please try again.';

          this.dialog.showError('Delete failed', message, undefined, 'error');
        },
      });
    });
  }
}
