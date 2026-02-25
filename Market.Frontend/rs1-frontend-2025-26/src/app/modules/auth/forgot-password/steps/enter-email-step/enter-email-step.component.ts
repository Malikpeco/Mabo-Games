import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { UserApiService } from '../../../../../api-services/users/users-api.service';
import { FormBuilder, Validators } from '@angular/forms';
import { CheckRecoveryOptionsQuery, CheckRecoveryOptionsResultDto } from '../../../../../api-services/users/users-api.model';

@Component({
  selector: 'app-enter-email-step',
  standalone: false,
  templateUrl: './enter-email-step.component.html',
  styleUrl: './enter-email-step.component.scss',
  host: { style: 'width: 100%; height: 100%' }
})
export class EnterEmailStepComponent extends BaseComponent {


  private fb = inject(FormBuilder);
  private userApi = inject(UserApiService);




  @Output() emailSubmitted = new EventEmitter<{

    email: string,
    recoveryOptions: CheckRecoveryOptionsResultDto,

  }>();


  form = this.fb.group({
    email: ['sec@market.com', [Validators.required, Validators.email]],
  })


  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const recoveryPayload: CheckRecoveryOptionsQuery = {
      recoveryEmail: this.form.value.email!,
    }

    this.userApi.checkRecoveryOptions(recoveryPayload).subscribe({
      next: (result) => {
      
        this.emailSubmitted.emit({
          email: recoveryPayload.recoveryEmail,
          recoveryOptions: result,
        });
      },
      error: (error) => {
        this.stopLoading("User with that email adress not found. Please try again.");
      }

    })

  }

}




