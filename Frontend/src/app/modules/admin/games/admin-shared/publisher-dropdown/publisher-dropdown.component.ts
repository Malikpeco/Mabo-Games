import { Component, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { PublishersApiService } from '../../../../../api-services/publishers/publishers-api.service';
import { ListPublishersRequest, PublisherAutocompleteDto } from '../../../../../api-services/publishers/publishers-api.models';
import { DialogHelperService } from '../../../../shared/services/dialog-helper.service';
import { CreatePublisherDialogComponent, CreatePublisherDialogResult } from '../create-publisher-dialog/create-publisher-dialog.component';

@Component({
  selector: 'app-publisher-dropdown',
  standalone: false,
  templateUrl: './publisher-dropdown.component.html',
  styleUrl: './publisher-dropdown.component.scss',
})
export class PublisherDropdownComponent implements OnInit, OnChanges {
  @Input() initialPublisherId: number | null = null;
  @Input() initialPublisherName = '';
  @Output() publisherSelected = new EventEmitter<PublisherAutocompleteDto | null>();

  private readonly minSearchLength = 2;

  searchTerm = '';
  allPublishers: PublisherAutocompleteDto[] = [];
  isOpen = false;
  isLoading = false;
  isCreating = false;
  error = '';
  selectedPublisherId: number | null = null;

  private publishersApi = inject(PublishersApiService);
  private dialog = inject(DialogHelperService);
  private matDialog = inject(MatDialog);
  private hostElement = inject(ElementRef<HTMLElement>);

  ngOnInit(): void {
    this.loadPublishers();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialPublisherId'] || changes['initialPublisherName']) {
      this.applyInitialSelection();
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target;
    if (!target || this.hostElement.nativeElement.contains(target as Node)) {
      return;
    }

    if (!(target as HTMLElement).closest('.publisher-dropdown-field')) {
      this.isOpen = false;
    }
  }

  get filteredPublishers(): PublisherAutocompleteDto[] {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      return this.allPublishers;
    }

    return this.allPublishers.filter((publisher) =>
      publisher.name.toLowerCase().includes(term),
    );
  }

  get canCreate(): boolean {
    const term = this.searchTerm.trim();
    return term.length >= this.minSearchLength
      && !this.isLoading
      && !this.isCreating
      && this.filteredPublishers.length === 0;
  }

  onSearch(value: string): void {
    this.searchTerm = value ?? '';
    this.isOpen = true;
    this.error = '';

    this.selectedPublisherId = null;
    this.publisherSelected.emit(null);
  }

  selectPublisher(publisher: PublisherAutocompleteDto): void {
    this.selectedPublisherId = publisher.id;
    this.searchTerm = publisher.name;
    this.isOpen = false;

    this.publisherSelected.emit(publisher);
  }

  clearSelection(): void {
    this.selectedPublisherId = null;
    this.searchTerm = '';
    this.error = '';
    this.isOpen = false;
    this.isCreating = false;

    this.publisherSelected.emit(null);
  }

  isSelected(publisher: PublisherAutocompleteDto): boolean {
    return this.selectedPublisherId === publisher.id;
  }

  createNew(): void {
    const initialTitle = this.searchTerm.trim();
    if (!initialTitle || !this.canCreate) {
      return;
    }

    const dialogRef = this.matDialog.open(CreatePublisherDialogComponent, {
      width: '760px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
      data: { initialTitle },
    });

    dialogRef.afterClosed().subscribe((result: CreatePublisherDialogResult | null | undefined) => {
      if (!result) {
        return;
      }

      this.createPublisher(result);
    });
  }

  private loadPublishers(): void {
    this.isLoading = true;
    this.error = '';

    const request = new ListPublishersRequest();
    this.publishersApi.listForDropdown(request).subscribe({
      next: (results) => {
        this.allPublishers = [...(results ?? [])]
          .filter((publisher) => !!publisher.name?.trim())
          .sort((a, b) => a.name.localeCompare(b.name));

        this.isLoading = false;
        this.applyInitialSelection();
      },
      error: () => {
        this.allPublishers = [];
        this.isLoading = false;
        this.error = 'Could not load publishers.';
      },
    });
  }

  private applyInitialSelection(): void {
    if (this.allPublishers.length === 0) {
      if (!this.isOpen) {
        this.searchTerm = this.initialPublisherName ?? '';
      }

      this.selectedPublisherId = this.initialPublisherId;
      return;
    }

    const byId = this.initialPublisherId == null
      ? undefined
      : this.allPublishers.find((publisher) => publisher.id === this.initialPublisherId);

    const initialName = (this.initialPublisherName ?? '').trim().toLowerCase();
    const byName = initialName
      ? this.allPublishers.find((publisher) => publisher.name.toLowerCase() === initialName)
      : undefined;

    const normalizedInitialName = this.normalizePublisherName(this.initialPublisherName);
    const byNormalizedName = normalizedInitialName
      ? this.allPublishers.find((publisher) => this.normalizePublisherName(publisher.name) === normalizedInitialName)
      : undefined;

    const partialMatches = normalizedInitialName
      ? this.allPublishers.filter((publisher) => {
        const normalizedPublisherName = this.normalizePublisherName(publisher.name);
        return normalizedPublisherName.includes(normalizedInitialName)
          || normalizedInitialName.includes(normalizedPublisherName);
      })
      : [];

    const bySinglePartialMatch = partialMatches.length === 1 ? partialMatches[0] : undefined;

    const selected = byId ?? byName ?? byNormalizedName ?? bySinglePartialMatch;
    this.selectedPublisherId = selected?.id ?? null;

    if (!this.isOpen) {
      this.searchTerm = selected?.name ?? this.initialPublisherName ?? '';
    }

    if (selected) {
      this.publisherSelected.emit(selected);
    }
  }

  private normalizePublisherName(value: string | null | undefined): string {
    return (value ?? '')
      .toLowerCase()
      .replace(/[^a-z0-9]+/g, ' ')
      .trim();
  }

  private createPublisher(result: CreatePublisherDialogResult): void {
    this.isCreating = true;
    this.error = '';

    this.publishersApi
      .create({
        name: result.title,
        countryId: result.countryId,
      })
      .subscribe({
        next: (newPublisherId) => {
          const selected: PublisherAutocompleteDto = {
            id: newPublisherId,
            name: result.title,
          };

          this.allPublishers = [...this.allPublishers, selected]
            .sort((a, b) => a.name.localeCompare(b.name));
          this.selectPublisher(selected);
          this.isCreating = false;
          this.dialog.showSuccess('Publisher created', `Publisher "${result.title}" was created successfully.`, undefined, 'check_circle');
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not create publisher.';

          this.error = message;
          this.isCreating = false;
          this.dialog.showError('Publisher creation failed', message, undefined, 'error');
        },
      });
  }
}
