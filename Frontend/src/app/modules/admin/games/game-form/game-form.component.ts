import { Component, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AbstractControl, FormBuilder, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
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
import { MatDialog } from '@angular/material/dialog';
import { FileUploadDialogComponent } from '../../../../shared/components/file-upload-dialog/file-upload-dialog.component';

interface IgdbMediaOption {
  key: string;
  url: string;
  kind: 'Screenshot' | 'Artwork' | 'Upload';
}

// Mirrors the "at least one genre" rule enforced by CreateGameCommandValidator/UpdateGameCommandValidator.
function atLeastOneGenreValidator(control: AbstractControl): ValidationErrors | null {
  const ids = control.value as number[] | null;
  return ids && ids.length > 0 ? null : { required: true };
}

// Mirrors the file requirement enforced by CreateGameCommandValidator (required, non-empty, <= max size)
// and UpdateGameCommandValidator (optional, but non-empty and <= max size when provided).
function gameFileValidator(isEditMode: () => boolean, maxSizeBytes: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const file = control.value as File | null;

    if (!file) {
      return isEditMode() ? null : { required: true };
    }

    if (file.size <= 0) {
      return { empty: true };
    }

    if (file.size > maxSizeBytes) {
      return { maxSize: true };
    }

    return null;
  };
}

@Component({
  selector: 'app-game-form',
  standalone: false,
  templateUrl: './game-form.component.html',
  styleUrl: './game-form.component.scss',
})
export class GameFormComponent {
  private static readonly maxFileSizeBytes = 5 * 1024 * 1024; // 5MB, matches BE validators

  private fb = inject(FormBuilder);
  private gamesApi = inject(GamesApiService);
  private screenshotsApi = inject(ScreenshotsApiService);
  private dialog = inject(DialogHelperService);
  private matDialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  readonly screenshotSlots = [1, 2, 3, 4, 5, 6];

  isEditMode = false;
  editingGameId: number | null = null;
  isLoadingGame = false;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    releaseDate: [new Date().toISOString().slice(0, 10), [Validators.required]],
    price: [0, [Validators.required, Validators.min(0)]],
    description: [''],
    publisherId: [null as number | null, [Validators.required]],
    genreIds: [[] as number[], [atLeastOneGenreValidator]],
    coverImageURL: ['', [Validators.required, Validators.pattern(/^https?:\/\/.*$/i)]],
    file: [null as File | null, [gameFileValidator(() => this.isEditMode, GameFormComponent.maxFileSizeBytes)]],
  });

  publisherName = '';
  selectedGenres: GenreDto[] = [];
  selectedGenreNames: string[] = [];
  coverPreviewUrl: string | null = null;
  screenshotPreviews: Array<string | null> = [null, null, null, null, null, null];
  activeScreenshotIndex = 0;

  igdbMediaOptions: IgdbMediaOption[] = [];
  uploadedMediaOptions: IgdbMediaOption[] = [];
  selectedMediaUrls: string[] = [];

  isFreeToPlay = false;
  isUploadingCover = false;
  isUploadingScreenshots = false;
  isSelectingGameFile = false;
  isSaving = false;
  selectedGameFile: File | null = null;
  existingGameFilePath: string | null = null;

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
    // Re-run the file validator now that isEditMode is true (it was evaluated once at form construction).
    this.form.get('file')?.updateValueAndValidity();
    this.loadGameForEdit(routeId);
  }

  ngOnDestroy(): void {}

  hasError(controlName: string, errorType?: string): boolean {
    const control = this.form.get(controlName);
    if (!control || !control.touched) {
      return false;
    }

    return errorType ? control.hasError(errorType) : control.invalid;
  }

  onIgdbSearchCleared(): void {
    this.igdbMediaOptions = [];
    this.selectedMediaUrls = this.selectedMediaUrls.filter((url) => this.uploadedMediaOptions.some((item) => item.url === url));
    this.syncSelectedMediaToScreenshotSlots();
  }

  onPublisherSelected(publisher: PublisherAutocompleteDto | null): void {
    const publisherId = publisher?.id ?? null;
    this.publisherName = publisher?.name ?? '';

    this.form.get('publisherId')?.setValue(publisherId);
    this.form.get('publisherId')?.markAsTouched();
  }

  onGenresChanged(genres: GenreDto[]): void {
    this.selectedGenres = genres ?? [];
    this.selectedGenreNames = this.selectedGenres.map((genre) => genre.name);

    const genreIds = this.selectedGenres.map((genre) => genre.id).filter((id): id is number => id > 0);
    this.form.get('genreIds')?.setValue(genreIds);
    this.form.get('genreIds')?.markAsTouched();
  }

  onIgdbDetailsSelected(details: GetIgdbGameDetailsDto): void {
    this.applyIgdbDetails(details);
  }

  private applyIgdbDetails(details: GetIgdbGameDetailsDto): void {
    this.form.get('name')?.setValue(details.name ?? this.form.get('name')?.value ?? '');

    if (details.releaseDate) {
      const parsedReleaseDate = this.toDateInputValue(details.releaseDate);
      if (parsedReleaseDate) {
        this.form.get('releaseDate')?.setValue(parsedReleaseDate);
      }
    }

    if (details.summary?.trim()) {
      this.form.get('description')?.setValue(details.summary);
    }

    if (details.publisher?.trim()) {
      this.publisherName = details.publisher;
      // IGDB only gives us a publisher name, not a real publisher id from our DB, so the field
      // is left invalid on purpose until the admin actually picks/creates a matching publisher.
      this.form.get('publisherId')?.setValue(null);
    }

    const incomingGenres = (details.genres ?? [])
      .map((genre) => genre.trim())
      .filter((genre) => genre.length > 0);

    if (incomingGenres.length > 0) {
      this.selectedGenreNames = incomingGenres;
      this.selectedGenres = [];
      this.form.get('genreIds')?.setValue([]);
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

    this.syncCoverImageControl();
  }

  // CoverImageURL isn't backed by a single input - it falls back to the first selected
  // screenshot/artwork when no explicit cover was uploaded/picked. Recompute it here so the
  // "coverImageURL" form control (and its validators) always reflect what will actually be sent.
  private syncCoverImageControl(): void {
    const validHttpUrls = this.selectedMediaUrls.filter((url) => /^https?:\/\//i.test(url));
    const coverCandidate = this.coverPreviewUrl ?? validHttpUrls[0] ?? '';
    this.form.get('coverImageURL')?.setValue(coverCandidate);
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
      this.syncCoverImageControl();
      this.form.get('coverImageURL')?.markAsTouched();
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

  openGameFileUploadDialog(): void {
    const ref = this.matDialog.open(FileUploadDialogComponent, {
      width: '560px',
      maxWidth: 'calc(100vw - 24px)',
      data: {
        managedUploading: true,
        title: 'Upload game file',
        subtitle: 'Choose the game build/archive file admins should attach to this title.',
        emptyStateTitle: 'Drop game file here',
        emptyStateSubtitle: 'Any file type up to 5 MB.',
        uploadButtonText: 'Use file',
        uploadingText: 'Selecting...',
        uploadIcon: 'upload_file',
        acceptedFileTypes: ['*/*'],
        maxFileSizeBytes: 5 * 1024 * 1024,
        showImagePreview: false,
      },
    });

    ref.componentInstance.fileSelected.subscribe((file: File) => {
      this.isSelectingGameFile = true;
      ref.componentInstance.errorMessage = '';

      try {
        this.selectedGameFile = file;
        this.form.get('file')?.setValue(file);
        this.form.get('file')?.markAsTouched();
        ref.close();
      } catch {
        ref.componentInstance.errorMessage = 'Could not select the chosen file.';
      } finally {
        this.isSelectingGameFile = false;
      }
    });
  }

  clearSelectedGameFile(): void {
    this.selectedGameFile = null;
    this.form.get('file')?.setValue(null);
    this.form.get('file')?.markAsTouched();
  }

  get selectedGameFileName(): string {
    if (this.selectedGameFile) {
      return this.selectedGameFile.name;
    }

    if (!this.existingGameFilePath) {
      return '';
    }

    const fileName = this.existingGameFilePath.split(/[\\/]/).pop();
    return fileName ?? this.existingGameFilePath;
  }

  setActiveScreenshot(index: number): void {
    this.activeScreenshotIndex = index;
  }

  get activeScreenshotUrl(): string | null {
    return this.screenshotPreviews[this.activeScreenshotIndex];
  }

  onFreeToPlayChange(isChecked: boolean): void {
    this.isFreeToPlay = isChecked;
    const priceControl = this.form.get('price');

    if (this.isFreeToPlay) {
      priceControl?.setValue(0);
      priceControl?.disable();
    } else {
      priceControl?.enable();
    }
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
    const releaseDate = this.form.get('releaseDate')?.value;
    if (!releaseDate) {
      return new Date().toISOString();
    }

    const parsed = new Date(`${releaseDate}T00:00:00`);
    if (Number.isNaN(parsed.getTime())) {
      return new Date().toISOString();
    }

    return parsed.toISOString();
  }

  private loadGameForEdit(gameId: number): void {
    this.isLoadingGame = true;

    this.gamesApi.getById(gameId).subscribe({
      next: (game) => {
        const price = Number(game.price ?? 0);

        this.form.patchValue({
          name: game.name ?? '',
          description: game.description ?? '',
          price,
          releaseDate: this.toDateInputValue(game.releaseDate) || this.form.get('releaseDate')?.value,
        });

        this.isFreeToPlay = price === 0;
        if (this.isFreeToPlay) {
          this.form.get('price')?.disable();
        }

        this.publisherName = game.publisher?.name ?? '';
        this.form.get('publisherId')?.setValue(game.publisher?.id ?? null);

        this.coverPreviewUrl = game.coverImageURL ?? null;
        this.existingGameFilePath = game.gameFilePath ?? null;

        this.selectedGenres = (game.genres ?? []).map((genre) => ({
          id: genre.id,
          name: genre.name,
        }));
        this.selectedGenreNames = this.selectedGenres.map((genre) => genre.name);
        this.form.get('genreIds')?.setValue(this.selectedGenres.map((genre) => genre.id));

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

    this.form.markAllAsTouched();

    if (this.form.invalid) {
      this.toaster.error('Please fix the highlighted fields before saving.');
      return;
    }

    const raw = this.form.getRawValue();
    const gameName = (raw.name ?? '').trim();
    const genreIds = (raw.genreIds ?? []).filter((id): id is number => id > 0);
    const validHttpUrls = this.selectedMediaUrls.filter((url) => /^https?:\/\//i.test(url));

    const createPayload: CreateGameRequest = {
      name: gameName,
      price: raw.price ?? 0,
      description: raw.description?.trim() || undefined,
      releaseDate: this.getReleaseDateIso(),
      publisherId: raw.publisherId!,
      coverImageURL: raw.coverImageURL ?? '',
      genreIds,
      screenshotUrls: validHttpUrls,
      file: this.selectedGameFile,
    };

    const updatePayload: UpdateGameRequest = {
      name: gameName,
      price: raw.price ?? 0,
      description: raw.description?.trim() || undefined,
      releaseDate: this.getReleaseDateIso(),
      publisherId: raw.publisherId!,
      coverImageURL: raw.coverImageURL ?? '',
      genreIds,
      screenshotUrls: validHttpUrls,
      file: this.selectedGameFile,
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
