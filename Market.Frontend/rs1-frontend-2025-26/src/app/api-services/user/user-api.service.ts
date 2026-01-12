import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CheckRecoveryOptionsQuery,
  CheckRecoveryOptionsResultDto,
  RegisterUserCommand
  , RegisterUserResultDto,
  RequestPasswordResetByEmailCommand,
  PasswordResetCommand,
  VerifyResetCodeQuery
} from './user-api.model';


//NOTE TO SELF. Pripazi koji http saljes, da li je put, get itd...

@Injectable({
  providedIn: 'root'
})
export class UserApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/users`;
  private http = inject(HttpClient);

  /**
   * POST /register
   * Registers a new user.
   */
  register(payload: RegisterUserCommand): Observable<RegisterUserResultDto> {
    return this.http.post<RegisterUserResultDto>(`${this.baseUrl}/register`, payload);
  }


  /**
 * GET /check-recovery-options
 * Retrivies recovery options for a given email.
 */

  checkRecoveryOptions(payload: CheckRecoveryOptionsQuery): Observable<CheckRecoveryOptionsResultDto> {
    return this.http.get<CheckRecoveryOptionsResultDto>(`${this.baseUrl}/check-recovery-options`, {
      params: { recoveryEmail: payload.recoveryEmail }
    });
  }


  /**
* POST /password-reset/email
* Sends a recovery code to reset password with by email.
*/
  requestPasswordResetByEmail(payload: RequestPasswordResetByEmailCommand): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/password-reset/email`, payload);
  }


  /**
 * GET /verify-reset-code
 * Verifies reset code for password recovery.
 */

  verifyResetCode(payload: VerifyResetCodeQuery): Observable<VerifyResetCodeQuery> {
    return this.http.get<VerifyResetCodeQuery>(`${this.baseUrl}/verify-reset-code`, {
      params: { resetCode: payload.resetCode, userEmail: payload.userEmail }
    });
  }


  /**
* PUT /password-reset/email
* Resets the user's password using the recovery code and then disables the token.
*/
  resetPassword(payload: PasswordResetCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/password-reset/change`, payload);
  }





}






