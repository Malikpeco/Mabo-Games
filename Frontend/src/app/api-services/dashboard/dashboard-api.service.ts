import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { buildHttpParams } from "../../core/models/build-http-params";
import { DashboardSummaryDto, GetDashboardSummaryRequest } from "./dashboard-api.models";

@Injectable({
    providedIn: 'root'
})
export class DashboardApiService {
    private http = inject(HttpClient);

    private readonly baseUrl = `${environment.apiUrl}/api/dashboard`;

    getSummary(request?: GetDashboardSummaryRequest): Observable<DashboardSummaryDto> {
        const params = request ? buildHttpParams(request as any) : undefined;
        return this.http.get<DashboardSummaryDto>(`${this.baseUrl}/summary`, { params });
    }
}
