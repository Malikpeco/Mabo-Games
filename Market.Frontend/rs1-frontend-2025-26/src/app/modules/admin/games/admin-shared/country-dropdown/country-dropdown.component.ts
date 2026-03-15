import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, inject } from '@angular/core';
import { CountriesApiService } from '../../../../../api-services/countries/countries-api.service';
import { CountryAutocompleteDto } from '../../../../../api-services/countries/countries-api.models';

@Component({
  selector: 'app-country-dropdown',
  standalone: false,
  templateUrl: './country-dropdown.component.html',
  styleUrl: './country-dropdown.component.scss',
})
export class CountryDropdownComponent implements OnInit {
  private readonly minSearchLength = 2;
  private readonly searchDebounceMs = 400;

  @Input() initialCountryName = '';
  @Output() countrySelected = new EventEmitter<CountryAutocompleteDto | null>();

  searchTerm = '';
  countryOptions: CountryAutocompleteDto[] = [];
  isOpen = false;
  isLoading = false;
  errorMessage = '';

  private countriesApi = inject(CountriesApiService);
  private hostElement = inject(ElementRef<HTMLElement>);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private requestSeq = 0;

  ngOnInit(): void {
    this.searchTerm = this.initialCountryName?.trim() ?? '';
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target;
    if (!target || this.hostElement.nativeElement.contains(target as Node)) {
      return;
    }

    this.isOpen = false;
  }

  onSearchInput(value: string): void {
    this.searchTerm = value ?? '';
    this.errorMessage = '';
    this.countrySelected.emit(null);

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    const trimmed = this.searchTerm.trim();
    if (trimmed.length < this.minSearchLength) {
      this.countryOptions = [];
      this.isOpen = false;
      this.isLoading = false;
      return;
    }

    this.isOpen = true;
    this.searchDebounceTimer = setTimeout(() => {
      this.searchCountries(trimmed);
    }, this.searchDebounceMs);
  }

  onFocus(): void {
    if (this.searchTerm.trim()) {
      this.isOpen = true;
    }
  }

  selectCountry(country: CountryAutocompleteDto): void {
    this.searchTerm = country.name;
    this.isOpen = false;
    this.countrySelected.emit(country);
  }

  private searchCountries(term: string): void {
    const requestId = ++this.requestSeq;
    this.isLoading = true;
    this.errorMessage = '';

    this.countriesApi.autocomplete(term).subscribe({
      next: (countries) => {
        if (requestId !== this.requestSeq) {
          return;
        }

        this.countryOptions = countries ?? [];
        this.isLoading = false;
      },
      error: () => {
        if (requestId !== this.requestSeq) {
          return;
        }

        this.countryOptions = [];
        this.isLoading = false;
        this.errorMessage = 'Could not load countries.';
      },
    });
  }
}
