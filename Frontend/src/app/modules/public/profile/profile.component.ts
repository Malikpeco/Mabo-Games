import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { CountryAutocompleteDto } from '../../../api-services/countries/countries-api.models';
import { UserApiService } from '../../../api-services/users/users-api.service';
import { GetUserProfileQueryDto, UpdateCurrentUserProfileCommand } from '../../../api-services/users/users-api.model';
import { ToasterService } from '../../../core/services/toaster.service';
import {
  UploadProfilePictureDialogComponent,
  UploadProfilePictureDialogResult,
} from './upload-profile-picture-dialog/upload-profile-picture-dialog.component';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  private currentUserService = inject(CurrentUserService);
  private userApi = inject(UserApiService);
  private toaster = inject(ToasterService);
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  currentUser = this.currentUserService.currentUser;
  isAuthenticated = this.currentUserService.isAuthenticated;

  profile: GetUserProfileQueryDto | null = null;
  isLoading = false;
  isEditing = false;
  isSaving = false;
  errorMessage = '';
  editErrorMessage = '';
  editUsername = '';
  editBio = '';
  selectedCountryId: number | null = null;
  selectedCountryName = '';

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params: ParamMap) => {
        this.loadProfile(params.get('username'));
      });
  }

  get initials(): string {
    const profileName = this.profile?.username ?? this.getCurrentUsername();

    if (!profileName) {
      return 'MG';
    }

    return profileName
      .split(/[._-]+/)
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0]?.toUpperCase() ?? '')
      .join('') || 'MG';
  }

  get displayName(): string {
    const profileName = this.profile?.username ?? this.getCurrentUsername();

    if (!profileName) {
      return 'Guest player';
    }

    return profileName.replace(/[._-]+/g, ' ');
  }

  get bio(): string {
    return this.profile?.bio?.trim() || 'No bio added yet.';
  }

  get country(): string {
    return this.profile?.country?.trim() || '';
  }

  get canSave(): boolean {
    return !this.isSaving && this.editUsername.trim().length >= 3;
  }

  startEdit(): void {
    if (!this.profile?.isOwnProfile) {
      return;
    }

    this.isEditing = true;
    this.editErrorMessage = '';
    this.editUsername = this.profile.username ?? '';
    this.editBio = this.profile.bio ?? '';
    this.selectedCountryId = this.profile.countryId ?? null;
    this.selectedCountryName = this.profile.country ?? '';
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.editErrorMessage = '';
    this.isSaving = false;
  }

  openProfilePictureDialog(): void {
    if (!this.profile?.isOwnProfile || this.isLoading || this.isSaving) {
      return;
    }

    const dialogRef = this.dialog.open<UploadProfilePictureDialogComponent, void, UploadProfilePictureDialogResult | null>(
      UploadProfilePictureDialogComponent,
      {
        width: '560px',
        maxWidth: 'calc(100vw - 24px)',
        disableClose: true,
      }
    );

    dialogRef.afterClosed().subscribe(result => {
      if (!result?.updated) {
        return;
      }

      this.toaster.success('Profile picture updated.');
      this.loadProfile(this.route.snapshot.paramMap.get('username'));
    });
  }

  onCountrySelected(country: CountryAutocompleteDto | null): void {
    if (country) {
      this.selectedCountryId = country.id;
      this.selectedCountryName = country.name;
      return;
    }

    if (this.selectedCountryId === null) {
      this.selectedCountryName = '';
    }
  }

  saveProfile(): void {
    const username = this.editUsername.trim();

    if (username.length < 3) {
      this.editErrorMessage = 'Username must be at least 3 characters long.';
      return;
    }

    this.isSaving = true;
    this.editErrorMessage = '';

    const payload: UpdateCurrentUserProfileCommand = {
      username,
      bio: this.editBio.trim(),
      countryId: this.selectedCountryId,
    };

    this.userApi.updateCurrentUserProfile(payload).subscribe({
      next: () => {
        this.isSaving = false;
        this.isEditing = false;
        this.loadProfile(this.route.snapshot.paramMap.get('username'));
      },
      error: () => {
        this.isSaving = false;
        this.editErrorMessage = 'Could not save your profile changes.';
      }
    });
  }

  private loadProfile(username: string | null): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.isEditing = false;

    const request = username
      ? this.userApi.getUserProfile(username)
      : this.userApi.getCurrentUserProfile();

    request.subscribe({
      next: profile => {
        this.profile = profile;
        this.isLoading = false;

        if (profile.isOwnProfile) {
          this.editUsername = profile.username ?? '';
          this.editBio = profile.bio ?? '';
          this.selectedCountryId = profile.countryId ?? null;
          this.selectedCountryName = profile.country ?? '';
          return;
        }

        this.editUsername = '';
        this.editBio = '';
        this.selectedCountryId = null;
        this.selectedCountryName = '';
      },
      error: () => {
        this.errorMessage = 'Could not load profile details.';
        this.isLoading = false;
      }
    });
  }

  private getCurrentUsername(): string {
    const user = this.currentUser();
    return user?.email?.split('@')[0]?.trim() ?? '';
  }
}