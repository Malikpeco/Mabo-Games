import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface CreateGenreDialogData {
  initialTitle: string;
  mode?: 'create' | 'edit';
}

export interface CreateGenreDialogResult {
  title: string;
}

@Component({
  selector: 'app-create-genre-dialog',
  standalone: false,
  templateUrl: './create-genre-dialog.component.html',
  styleUrl: './create-genre-dialog.component.scss',
})
export class CreateGenreDialogComponent {
  title = '';
  mode: 'create' | 'edit' = 'create';

  constructor(
    private dialogRef: MatDialogRef<CreateGenreDialogComponent, CreateGenreDialogResult | null>,
    @Inject(MAT_DIALOG_DATA) public data: CreateGenreDialogData,
  ) {
    this.title = data.initialTitle ?? '';
    this.mode = data.mode ?? 'create';
  }

  close(): void {
    this.dialogRef.close(null);
  }

  save(): void {
    const title = this.title.trim();
    if (!title) {
      return;
    }

    this.dialogRef.close({ title });
  }

  get canSave(): boolean {
    return this.title.trim().length >= 2;
  }

  get titleText(): string {
    return this.mode === 'edit' ? 'Edit Genre' : 'Create Genre';
  }

  get subtitleText(): string {
    return this.mode === 'edit'
      ? 'Update genre name.'
      : 'Add a new genre.';
  }

  get submitText(): string {
    return this.mode === 'edit' ? 'Save' : 'Create';
  }
}
