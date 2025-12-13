import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {forkJoin} from 'rxjs';
import {ProductFormService} from '../services/product-form.service';
import {BaseFormComponent} from '../../../../../core/components/base-classes/base-form-component';
import {GetProductByIdQueryDto, UpdateProductCommand} from '../../../../../api-services/products/products-api.models';
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
  selector: 'app-products-edit',
  standalone: false,
  templateUrl: './products-edit.component.html',
  styleUrl: './products-edit.component.scss',
  providers: [ProductFormService]
})
export class ProductsEditComponent
  extends BaseFormComponent<GetProductByIdQueryDto>
  implements OnInit {

  private api = inject(ProductsApiService);
  private categoriesApi = inject(ProductCategoriesApiService);
  private formService = inject(ProductFormService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  productId!: number;
  categories: ListProductCategoriesQueryDto[] = [];

  ngOnInit(): void {
    this.productId = +this.route.snapshot.params['id'];
    this.initForm(true); // Edit mode
  }

  protected loadData(): void {
    this.startLoading();

    // Load product and categories in parallel
    forkJoin({
      product: this.api.getById(this.productId),
      categories: this.categoriesApi.list({ onlyEnabled: true, paging: largePaging })
    }).subscribe({
      next: ({ product, categories }) => {
        this.model = product;
        this.categories = categories.items;
        this.form = this.formService.createProductForm(product);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load product');
        this.toaster.error('Product not found');
        console.error('Load product error:', err);
        this.router.navigate(['/admin/products']);
      }
    });
  }

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const command: UpdateProductCommand = {
      name: this.form.value.name,
      description: this.form.value.description,
      price: this.form.value.price,
      categoryId: this.form.value.categoryId
    };

    this.api.update(this.productId, command).subscribe({
      next: () => {
        this.stopLoading();
        this.toaster.success('Product updated successfully');
        this.router.navigate(['/admin/products']);
      },
      error: (err) => {
        this.stopLoading('Failed to update product');
        console.error('Update product error:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/products']);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}
