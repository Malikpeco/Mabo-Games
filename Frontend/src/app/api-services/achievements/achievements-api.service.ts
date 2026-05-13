import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import {
	CreateAchievementRequest,
	ListAchievementsRequest,
	ListAchievementsResponse,
	UpdateAchievementRequest,
} from './achievements-api.models';

@Injectable({
	providedIn: 'root',
})
export class AchievementsApiService {
	private http = inject(HttpClient);
	private readonly baseUrl = `${environment.apiUrl}/api/achievements`;

	list(request?: ListAchievementsRequest) {
		const params = request ? buildHttpParams(request as any) : undefined;
		return this.http.get<ListAchievementsResponse>(this.baseUrl, { params });
	}

	create(request: CreateAchievementRequest) {
		return this.http.post<void>(this.baseUrl, request);
	}

	uploadImage(formData: FormData) {
		return this.http.post(`${this.baseUrl}/upload-image`, formData, { responseType: 'text' });
	}

	update(id: number, request: UpdateAchievementRequest) {
		return this.http.put<void>(`${this.baseUrl}/${id}`, request);
	}

	delete(id: number) {
		return this.http.delete<void>(`${this.baseUrl}/${id}`);
	}
}
