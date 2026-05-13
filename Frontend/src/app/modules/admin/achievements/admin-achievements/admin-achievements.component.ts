import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { AchievementDto, ListAchievementsRequest } from '../../../../api-services/achievements/achievements-api.models';
import { AchievementsApiService } from '../../../../api-services/achievements/achievements-api.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import { CreateAchievementDialogComponent, CreateAchievementDialogResult } from '../create-achievement-dialog/create-achievement-dialog.component';

@Component({
  selector: 'app-admin-achievements',
  standalone: false,
  templateUrl: './admin-achievements.component.html',
  styleUrl: './admin-achievements.component.scss',
})
export class AdminAchievementsComponent implements OnInit, OnDestroy {
  private achievementsApi = inject(AchievementsApiService);
  private dialog = inject(DialogHelperService);
  private matDialog = inject(MatDialog);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private requestSeq = 0;

  achievements: AchievementDto[] = [];
  searchTerm = '';
  sort = 'nameAsc';
  readonly sortOptions: Array<{ value: string; label: string }> = [
    { value: 'nameAsc', label: 'Name: A-Z' },
    { value: 'nameDesc', label: 'Name: Z-A' },
  ];
  isLoading = false;
  page = 1;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 25, 50, 100];
  totalCount = 0;

  ngOnInit(): void {
    this.loadAchievements();
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

  get filteredAchievements(): AchievementDto[] {
    if (this.sort === 'nameDesc') {
      return [...this.achievements].sort((a, b) => b.name.localeCompare(a.name));
    }

    return [...this.achievements].sort((a, b) => a.name.localeCompare(b.name));
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
      this.loadAchievements();
    }, 250);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.page) {
      return;
    }

    this.page = page;
    this.loadAchievements();
  }

  onPageSizeChange(size: number | string): void {
    const parsedSize = Number(size);
    if (!Number.isFinite(parsedSize) || parsedSize <= 0 || parsedSize === this.pageSize) {
      return;
    }

    this.pageSize = parsedSize;
    this.page = 1;
    this.loadAchievements();
  }

  onAddAchievement(): void {
    const dialogRef = this.matDialog.open(CreateAchievementDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: {
        initialName: this.searchTerm.trim(),
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((result: CreateAchievementDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.achievementsApi.create(result).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Achievement created',
            `Achievement "${result.name}" was created successfully.`,
            undefined,
            'check_circle'
          );

          this.searchTerm = '';
          this.page = 1;
          this.loadAchievements();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not create achievement. Please try again.';

          this.dialog.showError('Create failed', message, undefined, 'error');
        },
      });
    });
  }

  onEditAchievement(achievement: AchievementDto): void {
    const dialogRef = this.matDialog.open(CreateAchievementDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: {
        initialName: achievement.name,
        initialDescription: achievement.description,
        initialImageURL: achievement.imageURL,
        mode: 'edit',
      },
    });

    dialogRef.afterClosed().subscribe((result: CreateAchievementDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.achievementsApi.update(achievement.id, result).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Achievement updated',
            `Achievement "${result.name}" was updated successfully.`,
            undefined,
            'check_circle'
          );

          this.loadAchievements();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not update achievement. Please try again.';

          this.dialog.showError('Update failed', message, undefined, 'error');
        },
      });
    });
  }

  onDeleteAchievement(achievement: AchievementDto): void {
    this.dialog.confirmDelete(achievement.name).subscribe((response) => {
      if (response?.button !== DialogButton.DELETE) {
        return;
      }

      this.achievementsApi.delete(achievement.id).subscribe({
        next: () => {
          this.dialog.showSuccess(
            'Achievement deleted',
            `Achievement "${achievement.name}" was deleted successfully.`,
            undefined,
            'check_circle'
          );

          const wasLastItemOnPage = this.achievements.length === 1;
          if (wasLastItemOnPage && this.page > 1) {
            this.page = this.page - 1;
          }

          this.loadAchievements();
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not delete achievement. Please try again.';

          this.dialog.showError('Delete failed', message, undefined, 'error');
        },
      });
    });
  }

  private loadAchievements(): void {
    const requestId = ++this.requestSeq;
    this.isLoading = true;

    const request: ListAchievementsRequest = {
      paging: { page: this.page, pageSize: this.pageSize },
      search: this.searchTerm.trim() || null,
    };

    this.achievementsApi.list(request).subscribe({
      next: (res) => {
        if (requestId !== this.requestSeq) {
          return;
        }

        const nextTotalCount = res.total ?? 0;
        const nextTotalPages = Math.max(1, Math.ceil(nextTotalCount / this.pageSize));
        if (this.page > nextTotalPages) {
          this.page = nextTotalPages;
          this.loadAchievements();
          return;
        }

        this.achievements = res.items ?? [];
        this.totalCount = nextTotalCount;
        this.isLoading = false;
      },
      error: () => {
        if (requestId !== this.requestSeq) {
          return;
        }

        this.achievements = [];
        this.totalCount = 0;
        this.isLoading = false;
      },
    });
  }

}
