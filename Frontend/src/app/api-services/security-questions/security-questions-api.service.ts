import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.staging";
import { HttpClient } from "@angular/common/http";
import { ListSecurityQuestionsQuery, ListSecurityQuestionsQueryDto } from "./security-questions-api.model";
import { Observable } from "rxjs";
import { PageResult } from "../../core/models/paging/page-result";

@Injectable({
    providedIn: 'root'
})
export class SecurityQuestionsApiService {
    private readonly baseUrl = `${environment.apiUrl}/api/security-questions`;
    private http = inject(HttpClient);

    /**
     * GET /
     * List all security questions with pagination.
     */
    listSecurityQuestions(query?: ListSecurityQuestionsQuery): Observable<PageResult<ListSecurityQuestionsQueryDto>> {
        const params: any = {};
        if (query?.page) params['page'] = query.page;
        if (query?.pageSize) params['pageSize'] = query.pageSize;
        
        return this.http.get<PageResult<ListSecurityQuestionsQueryDto>>(`${this.baseUrl}`, { params });
    }
}
