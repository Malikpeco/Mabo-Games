import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export interface AchievementDto {
	id: number;
	name: string;
	description?: string | null;
	imageURL: string;
}

export class ListAchievementsRequest extends BasePagedQuery {
	search?: string | null;
}

export type ListAchievementsResponse = PageResult<AchievementDto>;

export interface CreateAchievementRequest {
	name: string;
	description?: string | null;
	imageURL: string;
}

export interface UpdateAchievementRequest extends CreateAchievementRequest {}
