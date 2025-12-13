import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { ProductCategoriesApiService } from '../../../../api-services/product-categories/product-categories-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListProductCategoriesRequest,
  ListProductCategoriesQueryDto,
} from '../../../../api-services/product-categories/product-categories-api.model';
import { ProductCategoryUpsertComponent } from './product-category-upsert/product-category-upsert.component';

@Component({
  selector: 'app-product-categories',
  standalone: false,
  templateUrl: './product-categories.component.html',
  styleUrl: './product-categories.component.scss',
})
export class ProductCategoriesComponent
  extends BaseListPagedComponent<ListProductCategoriesQueryDto, ListProductCategoriesRequest>
  implements OnInit
{
  private api = inject(ProductCategoriesApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private dialogHelper = inject(DialogHelperService);

  displayedColumns: string[] = ['name', 'isEnabled', 'actions'];
  showOnlyEnabled = true;

  constructor() {
    super();
    this.request = new ListProductCategoriesRequest();
    this.request.onlyEnabled = true;
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load categories');
        console.error('Load categories error:', err);
      },
    });
  }

  // === Filters ===

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onToggleEnabledFilter(checked: boolean): void {
    this.showOnlyEnabled = checked;
    this.request.onlyEnabled = checked;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  // === CRUD Actions ===

  onCreate(): void {
    const dialogRef = this.dialog.open(ProductCategoryUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'product-category-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'create',
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.productCategory.showCreateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onEdit(category: ListProductCategoriesQueryDto): void {
    const dialogRef = this.dialog.open(ProductCategoryUpsertComponent, {
      width: '500px',
      maxWidth: '90vw',
      panelClass: 'product-category-dialog',
      autoFocus: true,
      disableClose: false,
      data: {
        mode: 'edit',
        categoryId: category.id,
      },
    });

    dialogRef.afterClosed().subscribe((success: boolean) => {
      if (success) {
        this.dialogHelper.productCategory.showUpdateSuccess().subscribe();
        this.loadPagedData();
      }
    });
  }

  onDelete(category: ListProductCategoriesQueryDto): void {
    this.dialogHelper.productCategory.confirmDelete(category.name).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(category);
      }
    });
  }

  private performDelete(category: ListProductCategoriesQueryDto): void {
    this.startLoading();

    this.api.delete(category.id).subscribe({
      next: () => {
        this.dialogHelper.productCategory.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        const errorMessage = this.extractErrorMessage(err);

        // Show error dialog instead of toast
        this.dialogHelper.showError(
          'DIALOGS.TITLES.ERROR',
          'PRODUCT_CATEGORIES.DIALOGS.ERROR_DELETE'
        ).subscribe();

        console.error('Delete category error:', err);
      },
    });
  }

  onToggleStatus(category: ListProductCategoriesQueryDto): void {
    this.startLoading();

    const apiAction = category.isEnabled
      ? this.api.disable(category.id)
      : this.api.enable(category.id);

    apiAction.subscribe({
      next: () => {
        const status = category.isEnabled ? 'disabled' : 'enabled';
        this.toaster.success(`Category ${status} successfully`);
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        const errorMessage = this.extractErrorMessage(err);

        if (err.status === 409) {
          // Business rule conflict - show dialog for important errors
          this.dialogHelper.showWarning(
            'DIALOGS.TITLES.WARNING',
            errorMessage || 'PRODUCT_CATEGORIES.DIALOGS.ERROR_TOGGLE',
            { name: category.name }
          ).subscribe();
        } else {
          // Generic error - just toast
          this.toaster.error(errorMessage || 'Failed to change category status');
        }

        console.error('Toggle status error:', err);
        this.loadPagedData();
      },
    });
  }

  /**
   * Extract user-friendly error message from HTTP error response
   */
  private extractErrorMessage(err: any): string | null {
    if (err?.error) {
      if (typeof err.error === 'string') {
        return err.error;
      }

      if (err.error.message) {
        return err.error.message;
      }

      if (err.error.title) {
        return err.error.title;
      }

      if (err.error.errors && typeof err.error.errors === 'object') {
        const errors = Object.values(err.error.errors).flat();
        if (errors.length > 0) {
          return errors.join(', ');
        }
      }
    }

    if (err?.message) {
      return err.message;
    }

    if (err?.statusText && err.statusText !== 'Unknown Error') {
      return err.statusText;
    }

    return null;
  }
}
