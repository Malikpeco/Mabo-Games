import { Component, ElementRef, HostListener, OnDestroy, OnInit, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { GenresApiService } from '../../../../api-services/genres/genres-api.service';
import { GamesApiService } from '../../../../api-services/games/games-api.service';
import { CreateGameRequest } from '../../../../api-services/games/games-api.models';
import { IgdbApiService } from '../../../../api-services/igdb/igdb-api.service';
import { GetIgdbGameDetailsDto, SearchIgdbGameDto } from '../../../../api-services/igdb/igdb-api.models';
import { PublisherAutocompleteDto } from '../../../../api-services/publishers/publishers-api.models';
import { GenreDto } from '../../../../api-services/genres/genres-api.models';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { Router } from '@angular/router';
import { ScreenshotsApiService } from '../../../../api-services/screenshots/screenshots-api.service';
import { firstValueFrom } from 'rxjs';

interface IgdbMediaOption {
  key: string;
  url: string;
  kind: 'Screenshot' | 'Artwork' | 'Upload';
}

@Component({
  selector: 'app-admin-games-add',
  standalone: false,
  templateUrl: './admin-games-add.component.html',
  styleUrl: './admin-games-add.component.scss',
})
export class AdminGamesAddComponent implements OnInit, OnDestroy {
  private igdbApi = inject(IgdbApiService);
  private gamesApi = inject(GamesApiService);
  private genresApi = inject(GenresApiService);
  private screenshotsApi = inject(ScreenshotsApiService);
  private dialog = inject(DialogHelperService);
  private toaster = inject(ToasterService);
  private router = inject(Router);
  private hostElement = inject(ElementRef<HTMLElement>);
  private igdbSearchDebounceTimer?: ReturnType<typeof setTimeout>;
  private igdbSearchRequestSeq = 0;
  private igdbDetailsRequestSeq = 0;
  readonly screenshotSlots = [1, 2, 3, 4, 5, 6];

  gameTitle = '';
  publisherName = '';
  price: number | null = null;
  description = '';

  selectedGenres: GenreDto[] = [];
  selectedGenreNames: string[] = [];
  coverPreviewUrl: string | null = null;
  screenshotPreviews: Array<string | null> = [null, null, null, null, null, null];
  activeScreenshotIndex = 0;

  igdbSearchTerm = '';
  igdbSearchResults: SearchIgdbGameDto[] = [];
  isIgdbSearching = false;
  igdbSearchError = '';
  isIgdbDropdownOpen = false;
  selectedIgdbGameId: number | null = null;
  igdbMediaOptions: IgdbMediaOption[] = [];
  uploadedMediaOptions: IgdbMediaOption[] = [];
  selectedMediaUrls: string[] = [];

  selectedPublisherId: number | null = null;
  isFreeToPlay = false;
  isUploadingCover = false;
  isUploadingScreenshots = false;
  isSaving = false;

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    if (this.igdbSearchDebounceTimer) {
      clearTimeout(this.igdbSearchDebounceTimer);
    }
  }

  onPublisherSelected(publisher: PublisherAutocompleteDto | null): void {
    if (!publisher) {
      this.selectedPublisherId = null;
      this.publisherName = '';
      return;
    }

    this.selectedPublisherId = publisher.id;
    this.publisherName = publisher.name;
  }

  onGenresChanged(genres: GenreDto[]): void {
    this.selectedGenres = genres ?? [];
    this.selectedGenreNames = this.selectedGenres.map((genre) => genre.name);
  }

  onIgdbSearchInput(value: string): void {
    this.igdbSearchTerm = value ?? '';
    this.igdbSearchError = '';

    if (this.igdbSearchDebounceTimer) {
      clearTimeout(this.igdbSearchDebounceTimer);
    }

    const trimmed = this.igdbSearchTerm.trim();
    if (trimmed.length < 2) {
      this.igdbSearchResults = [];
      this.isIgdbSearching = false;
      this.isIgdbDropdownOpen = false;
      return;
    }

    this.isIgdbDropdownOpen = true;

    this.igdbSearchDebounceTimer = setTimeout(() => {
      this.runIgdbSearch(trimmed);
    }, 250);
  }

  clearIgdbSearch(): void {
    this.igdbSearchTerm = '';
    this.igdbSearchResults = [];
    this.igdbSearchError = '';
    this.isIgdbSearching = false;
    this.isIgdbDropdownOpen = false;
    this.igdbMediaOptions = [];
    this.selectedMediaUrls = this.selectedMediaUrls.filter((url) => this.uploadedMediaOptions.some((item) => item.url === url));
    this.syncSelectedMediaToScreenshotSlots();
  }

  onIgdbSearchFocus(): void {
    if (this.igdbSearchTerm.trim().length >= 2) {
      this.isIgdbDropdownOpen = true;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target;
    if (!target) {
      return;
    }

    const targetElement = target as HTMLElement;

    if (!targetElement.closest('.igdb-search-strip')) {
      this.isIgdbDropdownOpen = false;
    }
  }

  selectIgdbGame(game: SearchIgdbGameDto): void {
    this.selectedIgdbGameId = game.id;
    this.igdbSearchError = '';
    this.isIgdbDropdownOpen = false;

    const requestId = ++this.igdbDetailsRequestSeq;
    this.igdbApi.getGameDetails(game.id).subscribe({
      next: (details) => {
        if (requestId !== this.igdbDetailsRequestSeq) {
          return;
        }

        this.applyIgdbDetails(details);
      },
      error: () => {
        if (requestId !== this.igdbDetailsRequestSeq) {
          return;
        }

        this.igdbSearchError = 'Could not load selected game details.';
      },
    });
  }

  private runIgdbSearch(searchTerm: string): void {
    const requestId = ++this.igdbSearchRequestSeq;
    this.isIgdbSearching = true;
    this.igdbSearchError = '';

    this.igdbApi.searchGames(searchTerm).subscribe({
      next: (results) => {
        if (requestId !== this.igdbSearchRequestSeq) {
          return;
        }

        this.igdbSearchResults = results ?? [];
        this.isIgdbSearching = false;
      },
      error: () => {
        if (requestId !== this.igdbSearchRequestSeq) {
          return;
        }

        this.igdbSearchResults = [];
        this.isIgdbSearching = false;
        this.igdbSearchError = 'IGDB search failed. Try again.';
      },
    });
  }

  private applyIgdbDetails(details: GetIgdbGameDetailsDto): void {
    this.gameTitle = details.name ?? this.gameTitle;

    if (details.summary?.trim()) {
      this.description = details.summary;
    }

    if (details.publisher?.trim()) {
      this.publisherName = details.publisher;
      this.selectedPublisherId = null;
    }

    const incomingGenres = (details.genres ?? [])
      .map((genre) => genre.trim())
      .filter((genre) => genre.length > 0);

    if (incomingGenres.length > 0) {
      this.selectedGenreNames = incomingGenres;
      this.selectedGenres = [];
    }

    if (details.coverUrl) {
      this.coverPreviewUrl = details.coverUrl;
    }

    this.setIgdbMediaOptions(details);
    this.selectedMediaUrls = this.igdbMediaOptions
      .slice(0, this.screenshotSlots.length)
      .map((item) => item.url);
    this.syncSelectedMediaToScreenshotSlots();
  }

  toggleMediaSelection(media: IgdbMediaOption): void {
    const isSelected = this.selectedMediaUrls.includes(media.url);

    if (isSelected) {
      this.selectedMediaUrls = this.selectedMediaUrls.filter((url) => url !== media.url);
      this.syncSelectedMediaToScreenshotSlots();
      return;
    }

    if (this.selectedMediaUrls.length >= this.screenshotSlots.length) {
      return;
    }

    this.selectedMediaUrls = [...this.selectedMediaUrls, media.url];
    this.syncSelectedMediaToScreenshotSlots();
  }

  isMediaSelected(media: IgdbMediaOption): boolean {
    return this.selectedMediaUrls.includes(media.url);
  }

  trackByIgdbMediaKey(_: number, media: IgdbMediaOption): string {
    return media.key;
  }

  get mediaOptionsForGrid(): IgdbMediaOption[] {
    return this.getAllMediaOptions();
  }

  private setIgdbMediaOptions(details: GetIgdbGameDetailsDto): void {
    const screenshotItems = (details.screenshots ?? [])
      .filter((url) => !!url)
      .map((url, index) => ({
        key: `screenshot-${index}`,
        url,
        kind: 'Screenshot' as const,
      }));

    const artworkItems = (details.artworks ?? [])
      .filter((url) => !!url)
      .map((url, index) => ({
        key: `artwork-${index}`,
        url,
        kind: 'Artwork' as const,
      }));

    const dedupe = new Set<string>();
    this.igdbMediaOptions = [...screenshotItems, ...artworkItems].filter((item) => {
      if (dedupe.has(item.url)) {
        return false;
      }

      dedupe.add(item.url);
      return true;
    });

    this.selectedMediaUrls = this.selectedMediaUrls.filter((url) => this.getAllMediaOptions().some((item) => item.url === url));
  }

  private syncSelectedMediaToScreenshotSlots(): void {
    const selectedInOrder = this.selectedMediaUrls
      .filter((url) => this.getAllMediaOptions().some((item) => item.url === url))
      .slice(0, this.screenshotSlots.length);

    this.screenshotPreviews = Array.from(
      { length: this.screenshotSlots.length },
      (_, index) => selectedInOrder[index] ?? null,
    );

    if (!this.screenshotPreviews[this.activeScreenshotIndex]) {
      const firstFilledIndex = this.screenshotPreviews.findIndex((item) => item !== null);
      this.activeScreenshotIndex = firstFilledIndex === -1 ? 0 : firstFilledIndex;
    }
  }

  private getAllMediaOptions(): IgdbMediaOption[] {
    return [...this.igdbMediaOptions, ...this.uploadedMediaOptions];
  }

  async onCoverSelected(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) {
      return;
    }

    this.isUploadingCover = true;

    try {
      const response = await firstValueFrom(this.screenshotsApi.uploadImage(file));
      if (!response?.url) {
        this.toaster.error('Cover upload did not return a valid image URL.');
        return;
      }

      this.coverPreviewUrl = response.url;
    } catch {
      this.toaster.error('Cover upload failed. Please try again.');
    } finally {
      this.isUploadingCover = false;
      input.value = '';
    }
  }

  async onScreenshotsSelected(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const files = input.files;
    if (!files?.length) {
      return;
    }

    this.isUploadingScreenshots = true;

    for (const file of Array.from(files)) {
      try {
        const response = await firstValueFrom(this.screenshotsApi.uploadImage(file));
        if (!response?.url) {
          this.toaster.error(`Upload for "${file.name}" did not return a valid image URL.`);
          continue;
        }

        this.addUploadedMediaOption(response.url);
      } catch {
        this.toaster.error(`Upload failed for "${file.name}". Please try again.`);
      }
    }

    this.syncSelectedMediaToScreenshotSlots();
    this.isUploadingScreenshots = false;
    input.value = '';
  }

  private addUploadedMediaOption(uploadedUrl: string): void {
    if (this.uploadedMediaOptions.some((item) => item.url === uploadedUrl)) {
      return;
    }

    this.uploadedMediaOptions = [
      ...this.uploadedMediaOptions,
      {
        key: `upload-${Date.now()}-${this.uploadedMediaOptions.length}`,
        url: uploadedUrl,
        kind: 'Upload',
      },
    ];

    if (this.selectedMediaUrls.length < this.screenshotSlots.length) {
      this.selectedMediaUrls = [...this.selectedMediaUrls, uploadedUrl];
    }
  }

  clearAllSelectedMedia(): void {
    this.selectedMediaUrls = [];
    this.syncSelectedMediaToScreenshotSlots();
  }

  setActiveScreenshot(index: number): void {
    this.activeScreenshotIndex = index;
  }

  get activeScreenshotUrl(): string | null {
    return this.screenshotPreviews[this.activeScreenshotIndex];
  }

  onFreeToPlayChange(isChecked: boolean): void {
    this.isFreeToPlay = isChecked;
    if (this.isFreeToPlay) {
      this.price = 0;
    }
  }

  get effectivePrice(): number | null {
    return this.isFreeToPlay ? 0 : this.price;
  }

  onSave(): void {
    if (this.isSaving) {
      return;
    }

    const gameName = this.gameTitle.trim();
    if (gameName.length < 2) {
      this.toaster.error('Please enter a valid game title.');
      return;
    }

    if (this.effectivePrice === null || this.effectivePrice < 0) {
      this.toaster.error('Please enter a valid price.');
      return;
    }

    if (!this.selectedPublisherId || this.selectedPublisherId <= 0) {
      this.toaster.error('Please choose a publisher from the dropdown.');
      return;
    }

    const genreIds = this.selectedGenres
      .map((genre) => genre.id)
      .filter((id): id is number => id > 0);

    if (genreIds.length === 0) {
      this.toaster.error('Please select at least one genre.');
      return;
    }

    const validHttpUrls = this.selectedMediaUrls.filter((url) => /^https?:\/\//i.test(url));
    const coverCandidate = this.coverPreviewUrl ?? validHttpUrls[0] ?? '';

    if (!/^https?:\/\//i.test(coverCandidate)) {
      this.toaster.error('Cover image must be a web URL (http/https). Pick one from IGDB media or set an online cover.');
      return;
    }

    const payload: CreateGameRequest = {
      name: gameName,
      price: this.effectivePrice,
      description: this.description?.trim() || undefined,
      releaseDate: new Date().toISOString(),
      publisherId: this.selectedPublisherId,
      coverImageURL: coverCandidate,
      genreIds,
      screenshotUrls: validHttpUrls,
    };

    this.isSaving = true;
    this.gamesApi.create(payload).subscribe({
      next: () => {
        this.isSaving = false;
        this.dialog.showSuccess('Game created', `Game "${gameName}" was created successfully.`, undefined, 'check_circle');
        this.router.navigate(['/admin/games']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        const message =
          error.error?.message ||
          error.error?.title ||
          'Could not create game. Please verify the form and try again.';
        this.toaster.error(message);
      },
    });
  }

}
