import { ChangeDetectorRef, Component, Inject, NgZone, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { FileUploadDialogComponent } from '../../../../shared/components/file-upload-dialog/file-upload-dialog.component';
import { AchievementsApiService } from '../../../../api-services/achievements/achievements-api.service';

export interface CreateAchievementDialogData {
  initialName: string;
  initialDescription?: string | null;
  initialImageURL?: string | null;
  mode?: 'create' | 'edit';
}

export interface CreateAchievementDialogResult {
  name: string;
  description: string;
  imageURL: string;
}

@Component({
  selector: 'app-create-achievement-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-achievement-dialog.component.html',
  styleUrl: './create-achievement-dialog.component.scss',
})
export class CreateAchievementDialogComponent {
  private fb = inject(FormBuilder);

  // Mirrors CreateAchievementCommandValidator/UpdateAchievementCommandValidator (Name/ImageURL NotEmpty).
  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: [''],
    imageURL: ['', [Validators.required]],
  });

  uploadErrorMessage = '';
  mode: 'create' | 'edit' = 'create';
  private matDialog = inject(MatDialog);
  private achievementsApi = inject(AchievementsApiService);
  private cdr = inject(ChangeDetectorRef);
  private zone = inject(NgZone);

  constructor(
    private dialogRef: MatDialogRef<CreateAchievementDialogComponent, CreateAchievementDialogResult | null>,
    @Inject(MAT_DIALOG_DATA) public data: CreateAchievementDialogData,
  ) {
    this.form.patchValue({
      name: data.initialName ?? '',
      description: data.initialDescription ?? '',
      imageURL: data.initialImageURL ?? '',
    });
    this.mode = data.mode ?? 'create';
  }

  hasError(controlName: string, errorType?: string): boolean {
    const control = this.form.get(controlName);
    if (!control || !control.touched) {
      return false;
    }

    return errorType ? control.hasError(errorType) : control.invalid;
  }

  close(): void {
    this.dialogRef.close(null);
  }

  save(): void {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return;
    }

    const { name, description, imageURL } = this.form.getRawValue();

    this.dialogRef.close({
      name: (name ?? '').trim(),
      description: (description ?? '').trim(),
      imageURL: (imageURL ?? '').trim(),
    });
  }

  get imageURL(): string {
    return this.form.get('imageURL')?.value ?? '';
  }

  openUploadDialog(): void {
    this.uploadErrorMessage = '';

    const ref = this.matDialog.open(FileUploadDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      data: { managedUploading: true }
    });

    ref.componentInstance.fileSelected.subscribe(async (file: File) => {
      ref.componentInstance.errorMessage = '';
      ref.componentInstance.isUploading = true;

      try {
        const form = new FormData();
        form.append('file', file, file.name);
        const uploadedUrl = await firstValueFrom(this.achievementsApi.uploadImage(form));
        this.zone.run(() => {
          if (typeof uploadedUrl === 'string') {
            const imageURL = this.unwrapQuotedJsonString(uploadedUrl).trim();
            this.form.get('imageURL')?.setValue(imageURL);
            this.form.get('imageURL')?.markAsTouched();
            this.uploadErrorMessage = '';
          }

          // Ensure parent dialog preview updates immediately after upload.
          this.cdr.detectChanges();
          ref.close();
        });
      } catch {
        ref.componentInstance.isUploading = false;
        ref.componentInstance.errorMessage = 'Could not upload the selected file.';
        this.uploadErrorMessage = 'Image upload failed. Please try again.';
      }
    });
  }

  private unwrapQuotedJsonString(value: string): string {
    if (value.startsWith('"') && value.endsWith('"')) {
      return value.slice(1, -1);
    }

    return value;
  }

  get titleText(): string {
    return this.mode === 'edit' ? 'Edit Achievement' : 'Create Achievement';
  }

  get subtitleText(): string {
    return this.mode === 'edit'
      ? 'Update the achievement details.'
      : 'Add a new achievement to the admin list.';
  }

  get submitText(): string {
    return this.mode === 'edit' ? 'Save' : 'Create';
  }
}
