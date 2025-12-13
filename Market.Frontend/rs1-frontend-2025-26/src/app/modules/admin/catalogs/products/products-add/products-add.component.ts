import {Component, inject, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {ProductFormService} from '../services/product-form.service';
import {CreateProductCommand, GetProductByIdQueryDto} from '../../../../../api-services/products/products-api.models';
import {BaseFormComponent} from '../../../../../core/components/base-classes/base-form-component';
import {ProductsApiService} from '../../../../../api-services/products/products-api.service';
import {
  ProductCategoriesApiService
} from '../../../../../api-services/product-categories/product-categories-api.service';
import {ToasterService} from '../../../../../core/services/toaster.service';
import {
  ListProductCategoriesQueryDto
} from '../../../../../api-services/product-categories/product-categories-api.model';
import {largePaging} from '../../../../../core/models/paging/paging-utils';

@Component({
  selector: 'app-products-add',
  standalone: false,
  templateUrl: './products-add.component.html',
  styleUrl: './products-add.component.scss',
  providers: [ProductFormService]
})
export class ProductsAddComponent
  extends BaseFormComponent<GetProductByIdQueryDto>
  implements OnInit {

  private api = inject(ProductsApiService);
  private categoriesApi = inject(ProductCategoriesApiService);
  private formService = inject(ProductFormService);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  categories: ListProductCategoriesQueryDto[] = [];

  ngOnInit(): void {
    this.initForm(false); // Add mode
    this.loadCategories();
  }

  protected loadData(): void {
    // Not needed in add mode
  }

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const command: CreateProductCommand = {
      name: this.form.value.name,
      description: this.form.value.description,
      price: this.form.value.price,
      categoryId: this.form.value.categoryId
    };

    this.api.create(command).subscribe({
      next: (productId) => {
        this.stopLoading();
        this.toaster.success('Product created successfully');
        this.router.navigate(['/admin/products']);
      },
      error: (err) => {
        this.stopLoading('Failed to create product');
        console.error('Create product error:', err);
      }
    });
  }

  private loadCategories(): void {
    this.categoriesApi.list({ onlyEnabled: true, paging: largePaging }).subscribe({
      next: (response) => {
        this.categories = response.items;
      },
      error: (err) => {
        this.toaster.error('Failed to load categories');
        console.error('Load categories error:', err);
      }
    });
  }

  protected override initForm(isEdit: boolean): void {
    super.initForm(isEdit);
    this.form = this.formService.createProductForm();
  }

  onCancel(): void {
    this.router.navigate(['/admin/products']);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}
