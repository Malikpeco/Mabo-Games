import { Component, EventEmitter, inject, Input, input, Output } from '@angular/core';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { UserApiService } from '../../../../../api-services/users/users-api.service';
import { FormBuilder } from '@angular/forms';
import { CheckRecoveryOptionsResultDto, RequestPasswordResetByEmailCommand } from '../../../../../api-services/users/users-api.model';

@Component({
  selector: 'app-choose-method-step',
  standalone: false,
  templateUrl: './choose-method-step.component.html',
  styleUrl: './choose-method-step.component.scss',
  host: { style: 'width: 100%; height: 100%' }
})
export class ChooseMethodStepComponent extends BaseComponent {


  private fb = inject(FormBuilder);

  private userApi = inject(UserApiService);


  @Input({ required: true }) recoveryOptions!: CheckRecoveryOptionsResultDto | null;
  @Input({ required: true }) userEmail!: string;

  @Output() methodChosen = new EventEmitter<'email' | 'security-question'>();


  isSendingEmail: boolean = false;  


  startEmailRecovery(): void {

    if (this.isLoading) return;

    this.isSendingEmail = true;  
    this.startLoading();

    const payload: RequestPasswordResetByEmailCommand = {
      userEmail: this.userEmail
    }

    this.userApi.requestPasswordResetByEmail(payload).subscribe({
      next: () => {
        this.isSendingEmail = false;  
        this.stopLoading();
        this.methodChosen.emit('email');
      },
      error: (error) => {
        this.stopLoading("Failed to send password reset email. Please try again later.");
      }
    });

  }


  //temp for now have to implement (AGAIN BECAUSE IM STUPID)
  startSecurityQuestionRecovery(): void {
    this.methodChosen.emit('security-question');
  }

}
