//NOTE TO SELF. Pripazi koji http saljes, da li je put, get itd...

import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.staging";
import { HttpClient } from "@angular/common/http";
import { 
    GetUserSecurityQuestionsByIdQuery, 
    GetUserSecurityQuestionsByIdQueryDto, 
    GetUserSecurityQuestionsListByEmailQuery, 
    GetUserSecurityQuestionsListByEmailQueryDto, 
    GetUserSecurityQuestionsListQueryDto, 
    RegisterUserSecurityQuestionCommand, 
    RemoveUserSecurityQuestionCommand, 
    UpdateUserSecurityQuestionCommand }
     from "./user-security-questions-api.mode";

import { Observable } from "rxjs/internal/Observable";
import { PasswordResetCommand, VerifyResetCodeQuery } from "../users/users-api.model";

@Injectable({
    providedIn: 'root'
})
export class UserSecurityQuestionsApiService {
    private readonly baseUrl = `${environment.apiUrl}/api/user-security-questions`;
    private http = inject(HttpClient);

    /**
     * POST /register
     * Registers a new user.
     */
    registerSecurityQuestion(payload: RegisterUserSecurityQuestionCommand): Observable<{ id: number }> {
        return this.http.post<{ id: number }>(`${this.baseUrl}/register`, payload);
    }


    /**
   * DELETE /{id}
   * Removes a users security question.
   */

    removeUserSecurityQuestion(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }


    /**
  * PUT /password-reset/{id}
  * Sends a recovery code to reset password with by email.
  */
    updateUserSecurityQuestion(id: number, payload: UpdateUserSecurityQuestionCommand): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
    }


    /**
     * GET /user-security-questions/{id}
     * Get a single category by ID.
     */
    getUserSecurityQuestionById(id: number): Observable<GetUserSecurityQuestionsByIdQueryDto> {
        return this.http.get<GetUserSecurityQuestionsByIdQueryDto>(`${this.baseUrl}/${id}`);
    }

    /**
     * GET /user-security-questions/list
     * List user security questions.
     */
    getUserSecurityQuestionsList(): Observable<GetUserSecurityQuestionsListQueryDto[]> {
        return this.http.get<GetUserSecurityQuestionsListQueryDto[]>(`${this.baseUrl}/list`);
    }

        /**
     * GET /user-security-questions/list-email
     * List user security questions by email.
     */
    getUserSecurityQuestionsListByEmail(payload: GetUserSecurityQuestionsListByEmailQuery): Observable<GetUserSecurityQuestionsListByEmailQueryDto[]> {
        return this.http.get<GetUserSecurityQuestionsListByEmailQueryDto[]>(`${this.baseUrl}/list-email`, {params: { userEmail: payload.userEmail }});
    }

    





}