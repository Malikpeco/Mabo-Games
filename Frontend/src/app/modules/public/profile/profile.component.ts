import { Component, OnInit, computed, inject } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { CurrentUserDto } from '../../../core/services/auth/current-user.dto';
import { UserApiService } from '../../../api-services/users/users-api.service';
import { GetUserProfileQueryDto } from '../../../api-services/users/users-api.model';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  private currentUserService = inject(CurrentUserService);
  private userApi = inject(UserApiService);

  currentUser = this.currentUserService.currentUser;
  isAuthenticated = this.currentUserService.isAuthenticated;

  profile: GetUserProfileQueryDto | null = null;
  isLoading = false;
  errorMessage = '';



  ngOnInit(): void {
    this.isLoading = true;
    this.userApi.getCurrentUserProfile().subscribe({
      next: profile => {
        this.profile = profile;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Could not load profile details.';
        this.isLoading = false;
      }
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

  get username(): string {
    return this.profile?.username ?? 'Not set';
  }

  get bio(): string {
    return this.profile?.bio?.trim() || 'No bio added yet.';
  }

  get country(): string {
    return this.profile?.country?.trim() || '';
  }

  private getCurrentUsername(): string {
    const user = this.currentUser();
    return user?.email?.split('@')[0]?.trim() ?? '';
  }


}