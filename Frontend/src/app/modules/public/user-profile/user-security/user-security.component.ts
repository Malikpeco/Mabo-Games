import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { UserApiService } from '../../../../api-services/users/users-api.service';
import { UserSecurityQuestionsApiService } from '../../../../api-services/user-security-questions/user-security-questions-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { AuthFacadeService } from '../../../../core/services/auth/auth-facade.service';
import { GetUserSecurityQuestionsListByEmailQueryDto, RegisterUserSecurityQuestionCommand } from '../../../../api-services/user-security-questions/user-security-questions-api.mode';
import { SecurityQuestionsApiService } from '../../../../api-services/security-questions/security-questions-api.service';
import { ListSecurityQuestionsQueryDto } from '../../../../api-services/security-questions/security-questions-api.model';
import { GetUserProfileQueryDto, ChangePasswordCommand } from '../../../../api-services/users/users-api.model';

@Component({
  selector: 'app-user-security',
  standalone: false,
  templateUrl: './user-security.component.html',
  styleUrl: './user-security.component.scss',
})
export class UserSecurityComponent implements OnInit {
  private userApi = inject(UserApiService);
  private userSecurityQuestionsApi = inject(UserSecurityQuestionsApiService);
  private securityQuestionsApi = inject(SecurityQuestionsApiService);
  private toaster = inject(ToasterService);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  profile: GetUserProfileQueryDto | null = null;
  allSecurityQuestions: ListSecurityQuestionsQueryDto[] = [];
  userSecurityQuestions: GetUserSecurityQuestionsListByEmailQueryDto[] = [];
  
  passwordForm: FormGroup;
  phoneNumberForm: FormGroup;
  securityQuestionForm: FormGroup;
  
  isLoadingProfile = false;
  isLoadingQuestions = false;
  isChangingPassword = false;
  isUpdatingPhoneNumber = false;
  isAddingSecurityQuestion = false;
  isDeletingAccount = false;
  
  errorMessage = '';
  showDeleteConfirm = false;
  expandedSections = { password: true, phone: true, security: true, danger: false };

  constructor() {
    this.passwordForm = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmNewPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });

    this.phoneNumberForm = this.fb.group({
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?[0-9]{10,}$/)]]
    });

    this.securityQuestionForm = this.fb.group({
      securityQuestionId: ['', [Validators.required]],
      securityQuestionAnswer: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit(): void {
    this.loadProfile();
    this.loadSecurityQuestions();
    this.loadUserSecurityQuestions();
  }

  private passwordMatchValidator(group: FormGroup): { [key: string]: any } | null {
    const password = group.get('newPassword')?.value;
    const confirm = group.get('confirmNewPassword')?.value;
    return password === confirm ? null : { passwordMismatch: true };
  }

  toggleSection(section: keyof typeof this.expandedSections): void {
    this.expandedSections[section] = !this.expandedSections[section];
  }

  get newPasswordValue(): string {
    return this.passwordForm.get('newPassword')?.value || '';
  }

  async loadProfile(): Promise<void> {
    this.isLoadingProfile = true;
    try {
      this.profile = await firstValueFrom(this.userApi.getCurrentUserProfile());
      this.phoneNumberForm.patchValue({
        phoneNumber: this.profile?.phoneNumber || ''
      });
    } catch (error) {
      this.errorMessage = 'Failed to load profile.';
    } finally {
      this.isLoadingProfile = false;
    }
  }

  async loadSecurityQuestions(): Promise<void> {
    this.isLoadingQuestions = true;
    try {
      const result = await firstValueFrom(this.securityQuestionsApi.listSecurityQuestions({ page: 1, pageSize: 100 }));
      this.allSecurityQuestions = result.items || [];
    } catch (error) {
      this.toaster.error('Failed to load security questions.');
    } finally {
      this.isLoadingQuestions = false;
    }
  }

  async loadUserSecurityQuestions(): Promise<void> {
    try {
      if (this.profile?.isOwnProfile && this.profile?.email) {
        const result = await firstValueFrom(
          this.userSecurityQuestionsApi.getUserSecurityQuestionsListByEmail({
            userEmail: this.profile.email
          })
        );
        this.userSecurityQuestions = result;
      }
    } catch (error) {
      this.toaster.error('Failed to load your security questions.');
    }
  }

  async changePassword(): Promise<void> {
    if (!this.passwordForm.valid) {
      this.toaster.error('Please fill in all required fields correctly.');
      return;
    }

    this.isChangingPassword = true;
    try {
      const command: ChangePasswordCommand = {
        oldPassword: this.passwordForm.get('currentPassword')?.value,
        newPassword: this.passwordForm.get('newPassword')?.value,
        confirmNewPassword: this.passwordForm.get('confirmNewPassword')?.value
      };

      await firstValueFrom(this.userApi.changePassword(command));
      this.toaster.success('Password changed successfully.');
      this.passwordForm.reset();
    } catch (error: any) {
      const errorMsg = error?.error?.message || 'Failed to change password.';
      this.toaster.error(errorMsg);
    } finally {
      this.isChangingPassword = false;
    }
  }

  async updatePhoneNumber(): Promise<void> {
    if (!this.phoneNumberForm.valid) {
      this.toaster.error('Please enter a valid phone number.');
      return;
    }

    this.isUpdatingPhoneNumber = true;
    try {
      const phoneNumber = this.phoneNumberForm.get('phoneNumber')?.value;
      await firstValueFrom(this.userApi.changePhoneNumber({ phoneNumber }));
      this.toaster.success('Phone number updated successfully.');
      this.profile = await firstValueFrom(this.userApi.getCurrentUserProfile());
    } catch (error: any) {
      const errorMsg = error?.error?.message || 'Failed to update phone number.';
      this.toaster.error(errorMsg);
    } finally {
      this.isUpdatingPhoneNumber = false;
    }
  }

  async addSecurityQuestion(): Promise<void> {
    if (!this.securityQuestionForm.valid) {
      this.toaster.error('Please select a security question and provide an answer.');
      return;
    }

    this.isAddingSecurityQuestion = true;
    try {
      const command: RegisterUserSecurityQuestionCommand = {
        securityQuestionId: this.securityQuestionForm.get('securityQuestionId')?.value,
        securityQuestionAnswer: this.securityQuestionForm.get('securityQuestionAnswer')?.value
      };

      await firstValueFrom(this.userSecurityQuestionsApi.registerSecurityQuestion(command));
      this.toaster.success('Security question added successfully.');
      this.securityQuestionForm.reset();
      await this.loadUserSecurityQuestions();
    } catch (error: any) {
      const errorMsg = error?.error?.message || 'Failed to add security question.';
      this.toaster.error(errorMsg);
    } finally {
      this.isAddingSecurityQuestion = false;
    }
  }

  async removeSecurityQuestion(id: number): Promise<void> {
    try {
      await firstValueFrom(this.userSecurityQuestionsApi.removeUserSecurityQuestion(id));
      this.toaster.success('Security question removed.');
      await this.loadUserSecurityQuestions();
    } catch (error: any) {
      const errorMsg = error?.error?.message || 'Failed to remove security question.';
      this.toaster.error(errorMsg);
    }
  }

  async deleteAccount(): Promise<void> {
    if (!this.showDeleteConfirm) {
      this.showDeleteConfirm = true;
      return;
    }

    this.toaster.error('Please confirm account deletion from the confirmation input.');
  }

  async confirmDeleteAccount(confirmationText: string): Promise<void> {
    if (!confirmationText || confirmationText !== 'DELETE MY ACCOUNT') {
      this.toaster.error('Please type DELETE MY ACCOUNT to confirm.');
      return;
    }

    this.isDeletingAccount = true;
    try {
      await firstValueFrom(this.userApi.deleteAccount({ confirmationText }));
      await firstValueFrom(this.auth.logout());
      this.toaster.success('Account deleted successfully.');
      this.showDeleteConfirm = false;
      await this.router.navigate(['/auth/login']);
    } catch (error: any) {
      const errorMsg = error?.error?.message || 'Failed to delete account.';
      this.toaster.error(errorMsg);
    } finally {
      this.isDeletingAccount = false;
    }
  }

  cancelDeleteAccount(): void {
    this.showDeleteConfirm = false;
  }

  get getSecurityQuestionsNotAdded(): ListSecurityQuestionsQueryDto[] {
    const addedIds = new Set(this.userSecurityQuestions.map(q => q.id));
    return this.allSecurityQuestions.filter(q => !addedIds.has(q.id));
  }
}
