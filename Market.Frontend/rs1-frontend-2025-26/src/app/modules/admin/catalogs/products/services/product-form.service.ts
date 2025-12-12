import { Injectable, inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import {GetProductByIdQueryDto} from '../../../../../api-services/products/products-api.models';

/**
 * Service for creating and managing product forms.
 * Provides reusable form creation with validation for Add and Edit components.
 */
@Injectable()
export class ProductFormService {
  private fb = inject(FormBuilder);

  /**
   * Create a product form with validation.
   * If product data is provided, form is pre-filled (edit mode).
   */
  createProductForm(product?: GetProductByIdQueryDto): FormGroup {
    return this.fb.group({
      name: [
        product?.name ?? '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100)
        ]
      ],
      description: [
        product?.description ?? '',
        [Validators.maxLength(500)]
      ],
      price: [
        product?.price ?? 0,
        [
          Validators.required,
          Validators.min(0.01),
          Validators.max(1000000)
        ]
      ],
      categoryId: [
        product?.categoryId ?? null,
        [Validators.required]
      ]
    });
  }

  /**
   * Get validation error message for a form control.
   */
  getErrorMessage(form: FormGroup, controlName: string): string {
    const control = form.get(controlName);
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    const errors = control.errors;

    if (errors['required']) {
      return 'This field is required';
    }
    if (errors['minlength']) {
      return `Minimum ${errors['minlength'].requiredLength} characters required`;
    }
    if (errors['maxlength']) {
      return `Maximum ${errors['maxlength'].requiredLength} characters allowed`;
    }
    if (errors['min']) {
      return `Minimum value is ${errors['min'].min}`;
    }
    if (errors['max']) {
      return `Maximum value is ${errors['max'].max}`;
    }
    if (errors['email']) {
      return 'Invalid email format';
    }

    return 'Invalid value';
  }
}
