import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";
import { StorefrontGameDto } from "../games/games-api.models";

export class ListFavouritesQueryRequest extends BasePagedQuery{

}

export type ListFavouritesQueryResponse = PageResult<StorefrontGameDto>;