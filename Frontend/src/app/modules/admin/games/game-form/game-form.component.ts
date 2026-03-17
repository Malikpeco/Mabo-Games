import { Component, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { GamesApiService } from '../../../../api-services/games/games-api.service';
import { CreateGameRequest, UpdateGameRequest } from '../../../../api-services/games/games-api.models';
import { GetIgdbGameDetailsDto } from '../../../../api-services/igdb/igdb-api.models';
import { PublisherAutocompleteDto } from '../../../../api-services/publishers/publishers-api.models';
import { GenreDto } from '../../../../api-services/genres/genres-api.models';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ScreenshotsApiService } from '../../../../api-services/screenshots/screenshots-api.service';
import { firstValueFrom } from 'rxjs';

interface IgdbMediaOption {
  key: string;
  url: string;
  kind: 'Screenshot' | 'Artwork' | 'Upload';
}

@Component({
  selector: 'app-game-form',
  standalone: false,
  templateUrl: './game-form.component.html',
  styleUrl: './game-form.component.scss',
})
export class GameFormComponent {
  private gamesApi = inject(GamesApiService);
  private screenshotsApi = inject(ScreenshotsApiService);
  private dialog = inject(DialogHelperService);
  private toaster = inject(ToasterService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  readonly screenshotSlots = [1, 2, 3, 4, 5, 6];

  isEditMode = false;
  editingGameId: number | null = null;
  isLoadingGame = false;

  gameTitle = '';
  releaseDate = new Date().toISOString().slice(0, 10);
  publisherName = '';
  price: number | null = null;
  description = '';

  selectedGenres: GenreDto[] = [];
  selectedGenreNames: string[] = [];
  coverPreviewUrl: string | null = null;
  screenshotPreviews: Array<string | null> = [null, null, null, null, null, null];
  activeScreenshotIndex = 0;

  igdbMediaOptions: IgdbMediaOption[] = [];
  uploadedMediaOptions: IgdbMediaOption[] = [];
  selectedMediaUrls: string[] = [];

  selectedPublisherId: number | null = null;
  isFreeToPlay = false;
  isUploadingCover = false;
  isUploadingScreenshots = false;
  isSaving = false;

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (!idParam) {
      this.isEditMode = false;
      return;
    }

    const routeId = Number(idParam);
    if (!Number.isFinite(routeId) || routeId <= 0) {
      this.toaster.error('Invalid game id for edit page.');
      this.router.navigate(['/admin/games']);
      return;
    }

    this.isEditMode = true;
    this.editingGameId = routeId;
    this.loadGameForEdit(routeId);
  }

  ngOnDestroy(): void {}

  onIgdbSearchCleared(): void {
    this.igdbMediaOptions = [];
    this.selectedMediaUrls = this.selectedMediaUrls.filter((url) => this.uploadedMediaOptions.some((item) => item.url === url));
    this.syncSelectedMediaToScreenshotSlots();
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

  onIgdbDetailsSelected(details: GetIgdbGameDetailsDto): void {
    this.applyIgdbDetails(details);
  }

  private applyIgdbDetails(details: GetIgdbGameDetailsDto): void {
    this.gameTitle = details.name ?? this.gameTitle;

    if (details.releaseDate) {
      const parsedReleaseDate = this.toDateInputValue(details.releaseDate);
      if (parsedReleaseDate) {
        this.releaseDate = parsedReleaseDate;
      }
    }

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

  private toDateInputValue(value: string | null | undefined): string {
    if (!value) {
      return '';
    }

    const parsed = new Date(value);
    if (Number.isNaN(parsed.getTime())) {
      return '';
    }

    return parsed.toISOString().slice(0, 10);
  }

  private getReleaseDateIso(): string {
    if (!this.releaseDate) {
      return new Date().toISOString();
    }

    const parsed = new Date(`${this.releaseDate}T00:00:00`);
    if (Number.isNaN(parsed.getTime())) {
      return new Date().toISOString();
    }

    return parsed.toISOString();
  }

  private loadGameForEdit(gameId: number): void {
    this.isLoadingGame = true;

    this.gamesApi.getById(gameId).subscribe({
      next: (game) => {
        this.gameTitle = game.name ?? '';
        this.description = game.description ?? '';
        this.price = Number(game.price ?? 0);
        this.isFreeToPlay = this.price === 0;
        this.publisherName = game.publisher?.name ?? '';
        this.selectedPublisherId = game.publisher?.id ?? null;
        this.coverPreviewUrl = game.coverImageURL ?? null;
        this.releaseDate = this.toDateInputValue(game.releaseDate) || this.releaseDate;

        this.selectedGenres = (game.genres ?? []).map((genre) => ({
          id: genre.id,
          name: genre.name,
        }));
        this.selectedGenreNames = this.selectedGenres.map((genre) => genre.name);

        const existingScreenshotUrls = (game.screenshots ?? [])
          .map((item) => item.imageURL)
          .filter((url): url is string => /^https?:\/\//i.test(url));

        this.uploadedMediaOptions = existingScreenshotUrls.map((url, index) => ({
          key: `existing-${index}`,
          url,
          kind: 'Upload' as const,
        }));
        this.selectedMediaUrls = existingScreenshotUrls.slice(0, this.screenshotSlots.length);
        this.syncSelectedMediaToScreenshotSlots();

        this.isLoadingGame = false;
      },
      error: () => {
        this.isLoadingGame = false;
        this.toaster.error('Could not load game details for editing.');
        this.router.navigate(['/admin/games']);
      },
    });
  }

  onSave(): void {
    if (this.isSaving || this.isLoadingGame) {
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

    const createPayload: CreateGameRequest = {
      name: gameName,
      price: this.effectivePrice,
      description: this.description?.trim() || undefined,
      releaseDate: this.getReleaseDateIso(),
      publisherId: this.selectedPublisherId,
      coverImageURL: coverCandidate,
      genreIds,
      screenshotUrls: validHttpUrls,
    };

    const updatePayload: UpdateGameRequest = {
      name: gameName,
      price: this.effectivePrice,
      description: this.description?.trim() || undefined,
      releaseDate: this.getReleaseDateIso(),
      publisherId: this.selectedPublisherId,
      coverImageURL: coverCandidate,
      genreIds,
      screenshotUrls: validHttpUrls,
    };

    this.isSaving = true;

    if (this.isEditMode && this.editingGameId) {
      this.gamesApi.update(this.editingGameId, updatePayload).subscribe({
        next: () => {
          this.isSaving = false;
          this.dialog.showSuccess('Game updated', `Game "${gameName}" was updated successfully.`, undefined, 'check_circle');
          this.router.navigate(['/admin/games']);
        },
        error: (error: HttpErrorResponse) => {
          this.isSaving = false;
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not update game. Please verify the form and try again.';
          this.toaster.error(message);
        },
      });
      return;
    }

    this.gamesApi.create(createPayload).subscribe({
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
