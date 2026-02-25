import { Component, inject } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
import { GenresApiService } from '../../../api-services/genres/genres-api.service';
import { GenreDto } from '../../../api-services/genres/genres-api.models';


type SortUi = 'recent' | 'priceAsc' | 'priceDesc' | 'nameAsc' | 'nameDesc' ;

@Component({
  selector: 'app-browse-games',
  standalone: false,
  templateUrl: './browse-games.component.html',
  styleUrl: './browse-games.component.scss',
})
export class BrowseGamesComponent {
  
  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;
  
  private authFacadeService = inject(AuthFacadeService);
  private router = inject(Router);
  
  private gamesApi = inject(GamesApiService);
  private genresApi = inject(GenresApiService);


  logout():void{
    this.router.navigate(['/auth/logout']);
  }


  search = '';
  sort: SortUi = 'recent';

  page = 1;
  pageSize = 16;
  totalCount = 0;

  games: StorefrontGameDto[] = [];
  filteredGames: StorefrontGameDto[] = [];

  genres: GenreDto[] = [];

  selectedGenreIds = new Set<number>();
  
  genreCounts: Record<number, number> = {};




  ngOnInit(): void {
    this.genresApi.list().subscribe(res=>{
      this.genres=res.items ?? [];
    });

    this.reload();
  }

  reload(): void {

    const sortForApi =
      this.sort === 'priceAsc' ? 'priceAsc'
      : this.sort === 'priceDesc' ? 'priceDesc'
      : this.sort === 'nameAsc' ? 'nameAsc'
      : this.sort === 'nameDesc' ? 'nameDesc'
      : 'recent';

    this.gamesApi
      .storefront({
        paging: { page: this.page, pageSize: this.pageSize },
        sort: sortForApi,
        search: this.search?.trim() || null,
        genreIds: this.selectedGenreIds.size? Array.from(this.selectedGenreIds) : null
      })
      .subscribe(res => {
        this.games = res.items ?? [];
        this.totalCount = (res as any).totalCount ?? (res as any).total ?? this.games.length;
        this.filteredGames= this.games;
      });
  }

  
  getGameImage(game: StorefrontGameDto): string {
    return game.coverImageURL ?? game.screenshots[0].imageURL ??
    '/carousel-placeholder-image.png';
  }




  isGenreSelected(id: number): boolean {
    return this.selectedGenreIds.has(id);
  }

  toggleGenre(id: number): void {
    if (this.selectedGenreIds.has(id)) this.selectedGenreIds.delete(id);
    else this.selectedGenreIds.add(id);

    this.goToPage(1);
    this.reload();
  }

  clearGenres(): void {
    this.selectedGenreIds.clear();
    this.page=1;
    this.reload();
  }

  onSortChanged(): void{
    this.page=1;
    this.reload();
  }  
  onSearchChanged(): void{
    this.page=1;
    this.reload();
  }



  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get pageNumbers(): number[] {
  
    const total = this.totalPages;
    const start = Math.max(1, this.page - 3);
    const end = Math.min(total, start + 6);
    const out: number[] = [];
    for (let i = start; i <= end; i++) out.push(i);
    return out;
  }

  goToPage(p: number): void {
    if (p < 1 || p > this.totalPages || p === this.page) return;
    this.page = p;
    this.reload();
  }





}
