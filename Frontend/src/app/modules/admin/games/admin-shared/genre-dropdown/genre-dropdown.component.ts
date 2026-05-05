import { Component, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { GenreDto } from '../../../../../api-services/genres/genres-api.models';
import { GenresApiService } from '../../../../../api-services/genres/genres-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../../shared/services/dialog-helper.service';
import { DialogButton, DialogType } from '../../../../shared/models/dialog-config.model';

@Component({
  selector: 'app-genre-dropdown',
  standalone: false,
  templateUrl: './genre-dropdown.component.html',
  styleUrl: './genre-dropdown.component.scss',
})
export class GenreDropdownComponent {
  @Input() initialSelectedGenres: string[] = [];
  @Output() genresChanged = new EventEmitter<GenreDto[]>();

  searchTerm = '';
  isOpen = false;
  isLoading = false;
  isCreating = false;
  createError = '';

  private genresApi = inject(GenresApiService);
  private toaster = inject(ToasterService);
  private dialog = inject(DialogHelperService);
  private hostElement = inject(ElementRef<HTMLElement>);

  private availableGenres: GenreDto[] = [];
  private selectedGenreIds = new Set<number>();

  ngOnInit(): void {
    this.loadGenres();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialSelectedGenres']) {
      this.applyInitialSelection();
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target;
    if (!target || this.hostElement.nativeElement.contains(target as Node)) {
      return;
    }

    this.isOpen = false;
    this.createError = '';
  }

  get filteredGenres(): GenreDto[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.availableGenres;
    }

    return this.availableGenres.filter((genre) => genre.name.toLowerCase().includes(term));
  }

  get canCreateFromSearch(): boolean {
    const term = this.searchTerm.trim();
    return term.length >= 2
      && !this.isLoading
      && !this.isCreating
      && this.filteredGenres.length === 0;
  }

  onSearchInput(value: string): void {
    this.searchTerm = value ?? '';
    this.isOpen = true;
    this.createError = '';
  }

  onFocus(): void {
    this.isOpen = true;
  }

  isGenreSelected(genre: GenreDto): boolean {
    return this.selectedGenreIds.has(genre.id);
  }

  toggleGenre(genre: GenreDto): void {
    if (this.selectedGenreIds.has(genre.id)) {
      this.selectedGenreIds.delete(genre.id);
    } else {
      this.selectedGenreIds.add(genre.id);
    }

    this.emitSelection();
  }

  createGenreFromSearch(): void {
    const genreName = this.searchTerm.trim();
    if (!genreName || !this.canCreateFromSearch) {
      return;
    }

    this.dialog
      .showCustom({
        type: DialogType.QUESTION,
        title: 'Create Genre',
        message: `Create new genre "${genreName}"?`,
        icon: 'category',
        buttons: [
          { type: DialogButton.CANCEL },
          { type: DialogButton.SAVE, label: 'Create', color: 'primary' },
        ],
      })
      .subscribe((result) => {
        if (result?.button !== DialogButton.SAVE) {
          return;
        }

        this.createGenre(genreName);
      });
  }

  private createGenre(genreName: string): void {

    this.isCreating = true;
    this.createError = '';

    this.genresApi.create(genreName).subscribe({
      next: (newGenreId) => {
        const createdGenre: GenreDto = {
          id: newGenreId,
          name: genreName,
        };

        this.availableGenres = [...this.availableGenres, createdGenre].sort((a, b) => a.name.localeCompare(b.name));
        this.selectedGenreIds.add(newGenreId);

        this.searchTerm = '';
        this.isOpen = false;
        this.isCreating = false;
        this.emitSelection();
      },
      error: (error: HttpErrorResponse) => {
        this.isCreating = false;
        const message =
          error.error?.message
          || error.error?.title
          || 'Could not create genre.';

        this.createError = message;
        this.toaster.error(message);
      },
    });
  }

  private loadGenres(): void {
    this.isLoading = true;

    this.genresApi.list({ paging: { page: 1, pageSize: 1000 } }).subscribe({
      next: (res) => {
        this.availableGenres = [...(res.items ?? [])]
          .filter((genre) => !!genre.name?.trim())
          .sort((a, b) => a.name.localeCompare(b.name));

        this.isLoading = false;
        this.applyInitialSelection();
      },
      error: () => {
        this.availableGenres = [];
        this.selectedGenreIds.clear();
        this.isLoading = false;
      },
    });
  }

  private applyInitialSelection(): void {
    if (this.availableGenres.length === 0) {
      return;
    }

    const incomingNames = (this.initialSelectedGenres ?? [])
      .map((genre) => genre.trim().toLowerCase())
      .filter((genreName) => genreName.length > 0);

    this.selectedGenreIds.clear();
    for (const genre of this.availableGenres) {
      if (incomingNames.includes(genre.name.toLowerCase())) {
        this.selectedGenreIds.add(genre.id);
      }
    }

    this.emitSelection();
  }

  private emitSelection(): void {
    const selected = this.availableGenres
      .filter((genre) => this.selectedGenreIds.has(genre.id))
      .sort((a, b) => a.name.localeCompare(b.name));

    this.genresChanged.emit(selected);
  }
}
