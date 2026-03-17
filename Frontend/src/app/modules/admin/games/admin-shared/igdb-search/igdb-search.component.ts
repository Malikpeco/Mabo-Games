import { Component, ElementRef, EventEmitter, HostListener, OnDestroy, Output, inject } from '@angular/core';
import { IgdbApiService } from '../../../../../api-services/igdb/igdb-api.service';
import { GetIgdbGameDetailsDto, SearchIgdbGameDto } from '../../../../../api-services/igdb/igdb-api.models';

@Component({
  selector: 'app-igdb-search',
  standalone: false,
  templateUrl: './igdb-search.component.html',
  styleUrl: './igdb-search.component.scss',
})
export class IgdbSearchComponent implements OnDestroy {
  @Output() gameDetailsSelected = new EventEmitter<GetIgdbGameDetailsDto>();
  @Output() searchCleared = new EventEmitter<void>();

  private igdbApi = inject(IgdbApiService);
  private hostElement = inject(ElementRef<HTMLElement>);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;
  private searchRequestSeq = 0;
  private detailsRequestSeq = 0;

  searchTerm = '';
  searchResults: SearchIgdbGameDto[] = [];
  isSearching = false;
  searchError = '';
  isDropdownOpen = false;
  selectedGameId: number | null = null;

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }
  }

  onSearchInput(value: string): void {
    this.searchTerm = value ?? '';
    this.searchError = '';

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    const trimmed = this.searchTerm.trim();
    if (trimmed.length < 2) {
      this.searchResults = [];
      this.isSearching = false;
      this.isDropdownOpen = false;
      return;
    }

    this.isDropdownOpen = true;
    this.searchDebounceTimer = setTimeout(() => {
      this.runSearch(trimmed);
    }, 250);
  }

  onSearchFocus(): void {
    if (this.searchTerm.trim().length >= 2) {
      this.isDropdownOpen = true;
    }
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.searchResults = [];
    this.searchError = '';
    this.isSearching = false;
    this.isDropdownOpen = false;
    this.selectedGameId = null;
    this.searchCleared.emit();
  }

  selectGame(game: SearchIgdbGameDto): void {
    this.selectedGameId = game.id;
    this.searchError = '';
    this.isDropdownOpen = false;

    const requestId = ++this.detailsRequestSeq;
    this.igdbApi.getGameDetails(game.id).subscribe({
      next: (details) => {
        if (requestId !== this.detailsRequestSeq) {
          return;
        }

        this.gameDetailsSelected.emit(details);
      },
      error: () => {
        if (requestId !== this.detailsRequestSeq) {
          return;
        }

        this.searchError = 'Could not load selected game details.';
      },
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target;
    if (!target || this.hostElement.nativeElement.contains(target as Node)) {
      return;
    }

    if (!(target as HTMLElement).closest('.igdb-search-strip')) {
      this.isDropdownOpen = false;
    }
  }

  private runSearch(searchTerm: string): void {
    const requestId = ++this.searchRequestSeq;
    this.isSearching = true;
    this.searchError = '';

    this.igdbApi.searchGames(searchTerm).subscribe({
      next: (results) => {
        if (requestId !== this.searchRequestSeq) {
          return;
        }

        this.searchResults = results ?? [];
        this.isSearching = false;
      },
      error: () => {
        if (requestId !== this.searchRequestSeq) {
          return;
        }

        this.searchResults = [];
        this.isSearching = false;
        this.searchError = 'IGDB search failed. Try again.';
      },
    });
  }
}
