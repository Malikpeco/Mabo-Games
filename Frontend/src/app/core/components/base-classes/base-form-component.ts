import { Directive, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseComponent } from './base-component';

/**
 * Base component for forms (Add/Edit).
 * Provides common functionality for form components.
 *
 * Usage:
 * 1. Extend this class
 * 2. Implement loadData() for edit mode
 * 3. Implement save() for submit logic
 * 4. Call initForm(isEdit) in ngOnInit
 */
@Directive()
export abstract class BaseFormComponent<TModel> extends BaseComponent {

  form!: FormGroup;
  isEditMode = false;
  model?: TModel;

  /**
   * Child components must implement this to load data in edit mode.
   */
  protected abstract loadData(): void;

  /**
   * Child components must implement this to handle form submission.
   */
  protected abstract save(): void;

  /**
   * Initialize form in add or edit mode.
   * Call this from ngOnInit in child component.
   */
  protected initForm(isEdit: boolean): void {
    this.isEditMode = isEdit;

    if (isEdit) {
      this.loadData();
    }
  }

  /**
   * Handle form submission.
   * Validates form before calling save().
   */
  onSubmit(): void {
    // Mark all controls as touched to show validation errors
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return;
    }

    this.save();
  }

  /**
   * Check if a form control has an error and is touched.
   */
  hasError(controlName: string, errorType?: string): boolean {
    const control = this.form.get(controlName);
    if (!control || !control.touched) {
      return false;
    }

    if (errorType) {
      return control.hasError(errorType);
    }

    return control.invalid;
  }

  /**
   * Get the value of a form control.
   */
  getControlValue(controlName: string): any {
    return this.form.get(controlName)?.value;
  }

  /**
   * Set the value of a form control.
   */
  setControlValue(controlName: string, value: any): void {
    this.form.get(controlName)?.setValue(value);
  }

  /**
   * Reset the form to initial state.
   */
  resetForm(): void {
    this.form.reset();
  }
}
