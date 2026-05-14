import { PageResult } from "../../core/models/paging/page-result";

export interface ListSecurityQuestionsQuery {
    page?: number;
    pageSize?: number;
}

export interface ListSecurityQuestionsQueryDto {
    id: number;
    question: string;
}
