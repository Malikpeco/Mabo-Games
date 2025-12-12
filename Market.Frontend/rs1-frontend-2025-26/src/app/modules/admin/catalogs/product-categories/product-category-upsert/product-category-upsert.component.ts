import { Component, inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { ProductCategoriesApiService } from '../../../../../api-services/product-categories/product-categories-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  GetProductCategoryByIdQueryDto,
  CreateProductCategoryCommand,
  UpdateProductCategoryCommand
} from '../../../../../api-services/product-categories/product-categories-api.model';
import {ProductCategoryFormService} from "../services/product-category-form.service";

export interface ProductCategoryDialogData {
  mode: 'create' | 'edit';
  categoryId?: number;
}

@Component({
  selector: 'app-product-category-upsert',
  standalone: false,
  templateUrl: './product-category-upsert.component.html',
  styleUrl: './product-category-upsert.component.scss',
  providers: [ProductCategoryFormService]
})
export class ProductCategoryUpsertComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<ProductCategoryUpsertComponent>);
  private data = inject<ProductCategoryDialogData>(MAT_DIALOG_DATA);
  private api = inject(ProductCategoriesApiService);
  private formService = inject(ProductCategoryFormService);
  private toaster = inject(ToasterService);

  form!: FormGroup;
  isLoading = false;
  isEditMode = false;
  title = '';

  ngOnInit(): void {
    this.isEditMode = this.data.mode === 'edit';
    this.title = this.isEditMode ? 'Edit Category' : 'New Category';

    if (this.isEditMode && this.data.categoryId) {
      this.loadCategory(this.data.categoryId);
    } else {
      this.form = this.formService.createCategoryForm();
    }
  }

  private loadCategory(id: number): void {
    this.isLoading = true;

    this.api.getById(id).subscribe({
      next: (category) => {
        this.form = this.formService.createCategoryForm(category);
        this.isLoading = false;
      },
      error: (err) => {
        this.toaster.error('Failed to load category');
        console.error('Load category error:', err);
        this.dialogRef.close(false);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    if (this.isEditMode && this.data.categoryId) {
      this.updateCategory();
    } else {
      this.createCategory();
    }
  }

  private createCategory(): void {
    const command: CreateProductCategoryCommand = {
      name: this.form.value.name.trim()
    };

    this.api.create(command).subscribe({
      next: () => {
        this.toaster.success('Category created successfully');
        this.dialogRef.close(true); // Signal success
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Create category error:', err);
      }
    });
  }

  private updateCategory(): void {
    const command: UpdateProductCategoryCommand = {
      name: this.form.value.name.trim()
    };

    this.api.update(this.data.categoryId!, command).subscribe({
      next: () => {
        this.toaster.success('Category updated successfully');
        this.dialogRef.close(true); // Signal success
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Update category error:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}
