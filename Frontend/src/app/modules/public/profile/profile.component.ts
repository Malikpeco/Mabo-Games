import { Component, OnInit, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { CountryAutocompleteDto } from '../../../api-services/countries/countries-api.models';
import { UserApiService } from '../../../api-services/users/users-api.service';
import { GetUserProfileQueryDto, UpdateCurrentUserProfileCommand } from '../../../api-services/users/users-api.model';
import { ToasterService } from '../../../core/services/toaster.service';

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

  currentUser = this.currentUserService.currentUser;
  isAuthenticated = this.currentUserService.isAuthenticated;

  profile: GetUserProfileQueryDto | null = null;
  isLoading = false;
  isEditing = false;
  isSaving = false;
  isUploadingProfileImage = false;
  errorMessage = '';
  editErrorMessage = '';
  profileImageErrorMessage = '';
  editUsername = '';
  editBio = '';
  selectedCountryId: number | null = null;
  selectedCountryName = '';

  ngOnInit(): void {
    this.loadProfile();
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
    if (!this.profile) {
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

  async onProfileImageSelected(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    this.profileImageErrorMessage = '';
    this.isUploadingProfileImage = true;

    try {
      await firstValueFrom(this.userApi.uploadProfileImage(file));
      this.toaster.success('Profile picture updated.');
      this.loadProfile();
    } catch {
      this.profileImageErrorMessage = 'Could not update your profile picture.';
    } finally {
      this.isUploadingProfileImage = false;
      input.value = '';
    }
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
        this.loadProfile();
      },
      error: () => {
        this.isSaving = false;
        this.editErrorMessage = 'Could not save your profile changes.';
      }
    });
  }

  private loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.userApi.getCurrentUserProfile().subscribe({
      next: profile => {
        this.profile = profile;
        this.isLoading = false;

        if (!this.isEditing) {
          this.editUsername = profile.username ?? '';
          this.editBio = profile.bio ?? '';
          this.selectedCountryId = profile.countryId ?? null;
          this.selectedCountryName = profile.country ?? '';
        }
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