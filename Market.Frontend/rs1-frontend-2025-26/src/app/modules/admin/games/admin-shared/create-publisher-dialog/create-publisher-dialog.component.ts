import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CountryAutocompleteDto } from '../../../../../api-services/countries/countries-api.models';

export interface CreatePublisherDialogData {
  initialTitle: string;
}

export interface CreatePublisherDialogResult {
  title: string;
  countryId: number;
  countryName: string;
}

@Component({
  selector: 'app-create-publisher-dialog',
  standalone: false,
  templateUrl: './create-publisher-dialog.component.html',
  styleUrl: './create-publisher-dialog.component.scss',
})
export class CreatePublisherDialogComponent {
  title = '';
  selectedCountryId: number | null = null;
  selectedCountryName = '';

  constructor(
    private dialogRef: MatDialogRef<CreatePublisherDialogComponent, CreatePublisherDialogResult | null>,
    @Inject(MAT_DIALOG_DATA) public data: CreatePublisherDialogData,
  ) {
    this.title = data.initialTitle ?? '';
  }

  onCountrySelected(country: CountryAutocompleteDto | null): void {
    if (!country) {
      this.selectedCountryId = null;
      this.selectedCountryName = '';
      return;
    }

    this.selectedCountryId = country.id;
    this.selectedCountryName = country.name;
  }

  close(): void {
    this.dialogRef.close(null);
  }

  save(): void {
    const title = this.title.trim();
    if (!title || !this.selectedCountryId) {
      return;
    }

    this.dialogRef.close({
      title,
      countryId: this.selectedCountryId,
      countryName: this.selectedCountryName,
    });
  }

  get canSave(): boolean {
    return this.title.trim().length >= 2 && !!this.selectedCountryId;
  }
}
