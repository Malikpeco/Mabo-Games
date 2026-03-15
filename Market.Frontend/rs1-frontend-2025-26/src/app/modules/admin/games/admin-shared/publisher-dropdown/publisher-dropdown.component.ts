import { Component, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges, inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { PublishersApiService } from '../../../../../api-services/publishers/publishers-api.service';
import { ListPublishersRequest, PublisherAutocompleteDto } from '../../../../../api-services/publishers/publishers-api.models';
import { DialogHelperService } from '../../../../shared/services/dialog-helper.service';
import { DialogButton, DialogType } from '../../../../shared/models/dialog-config.model';
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

  internalSearchTerm = '';
  internalAllPublishers: PublisherAutocompleteDto[] = [];
  internalIsOpen = false;
  internalIsLoading = false;
  internalIsCreating = false;
  internalError = '';
  internalSelectedPublisherId: number | null = null;

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
      this.internalIsOpen = false;
    }
  }

  get internalFilteredPublishers(): PublisherAutocompleteDto[] {
    const term = this.internalSearchTerm.trim().toLowerCase();
    if (!term) {
      return this.internalAllPublishers;
    }

    return this.internalAllPublishers.filter((publisher) =>
      publisher.name.toLowerCase().includes(term),
    );
  }

  get internalCanCreate(): boolean {
    const term = this.internalSearchTerm.trim();
    return term.length >= this.minSearchLength
      && !this.internalIsLoading
      && !this.internalIsCreating
      && this.internalFilteredPublishers.length === 0;
  }

  onInternalSearch(value: string): void {
    this.internalSearchTerm = value ?? '';
    this.internalIsOpen = true;
    this.internalError = '';

    this.internalSelectedPublisherId = null;
    this.publisherSelected.emit(null);
  }

  selectPublisher(publisher: PublisherAutocompleteDto): void {
    this.internalSelectedPublisherId = publisher.id;
    this.internalSearchTerm = publisher.name;
    this.internalIsOpen = false;

    this.publisherSelected.emit(publisher);
  }

  clearSelection(): void {
    this.internalSelectedPublisherId = null;
    this.internalSearchTerm = '';
    this.internalError = '';
    this.internalIsOpen = false;
    this.internalIsCreating = false;

    this.publisherSelected.emit(null);
  }

  isSelected(publisher: PublisherAutocompleteDto): boolean {
    return this.internalSelectedPublisherId === publisher.id;
  }

  createNew(): void {
    const initialTitle = this.internalSearchTerm.trim();
    if (!initialTitle || !this.internalCanCreate) {
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

      this.dialog
        .showCustom({
          type: DialogType.QUESTION,
          title: 'Create Publisher',
          message: `Create publisher "${result.title}"?`,
          icon: 'business',
          buttons: [
            { type: DialogButton.CANCEL },
            { type: DialogButton.SAVE, label: 'Create', color: 'primary' },
          ],
        })
        .subscribe((confirmResult) => {
          if (confirmResult?.button !== DialogButton.SAVE) {
            return;
          }

          this.createPublisher(result);
        });
    });
  }

  private loadPublishers(): void {
    this.internalIsLoading = true;
    this.internalError = '';

    const request = new ListPublishersRequest();
    this.publishersApi.listForDropdown(request).subscribe({
      next: (results) => {
        this.internalAllPublishers = [...(results ?? [])]
          .filter((publisher) => !!publisher.name?.trim())
          .sort((a, b) => a.name.localeCompare(b.name));

        this.internalIsLoading = false;
        this.applyInitialSelection();
      },
      error: () => {
        this.internalAllPublishers = [];
        this.internalIsLoading = false;
        this.internalError = 'Could not load publishers.';
      },
    });
  }

  private applyInitialSelection(): void {
    if (this.internalAllPublishers.length === 0) {
      if (!this.internalIsOpen) {
        this.internalSearchTerm = this.initialPublisherName ?? '';
      }

      this.internalSelectedPublisherId = this.initialPublisherId;
      return;
    }

    const byId = this.initialPublisherId == null
      ? undefined
      : this.internalAllPublishers.find((publisher) => publisher.id === this.initialPublisherId);

    const initialName = (this.initialPublisherName ?? '').trim().toLowerCase();
    const byName = initialName
      ? this.internalAllPublishers.find((publisher) => publisher.name.toLowerCase() === initialName)
      : undefined;

    const selected = byId ?? byName;
    this.internalSelectedPublisherId = selected?.id ?? null;

    if (!this.internalIsOpen) {
      this.internalSearchTerm = selected?.name ?? this.initialPublisherName ?? '';
    }
  }

  private createPublisher(result: CreatePublisherDialogResult): void {
    this.internalIsCreating = true;
    this.internalError = '';

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

          this.internalAllPublishers = [...this.internalAllPublishers, selected]
            .sort((a, b) => a.name.localeCompare(b.name));
          this.selectPublisher(selected);
          this.internalIsCreating = false;
          this.dialog.showSuccess('Publisher created', `Publisher "${result.title}" was created successfully.`, undefined, 'check_circle');
        },
        error: (error: HttpErrorResponse) => {
          const message =
            error.error?.message ||
            error.error?.title ||
            'Could not create publisher.';

          this.internalError = message;
          this.internalIsCreating = false;
          this.dialog.showError('Publisher creation failed', message, undefined, 'error');
        },
      });
  }
}
