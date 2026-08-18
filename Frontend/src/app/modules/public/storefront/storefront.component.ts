import { Component, inject } from '@angular/core';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
import { GenresApiService } from '../../../api-services/genres/genres-api.service';
import { GenreDto } from '../../../api-services/genres/genres-api.models';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';

const GENRE_IMAGES: Record<string, string> = {
  'Action': 'https://www.theouterhaven.net/wp-content/uploads/2020/02/doom-eternal-2020-top-625x352-1.jpg',
  'Role-Playing (RPG)': 'https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.jpg',
  'Adventure': 'https://images.igdb.com/igdb/image/upload/t_cover_big/co1q1f.jpg',
  'Sports': 'https://legacymedia.sportsplatform.io/img/images/photos/003/757/965/75da9a20a992ae7b8b1d18f6ee3fb8a4_crop_north.jpg?w=802',
  'Strategy': 'https://upload.wikimedia.org/wikipedia/en/8/8f/Metal_Gear_Solid_V_The_Phantom_Pain_cover.png',
  'Open-World': 'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg',
  'Survival': 'https://upload.wikimedia.org/wikipedia/en/2/2c/Resident_Evil_Village.png',
  'Horror': 'https://images.igdb.com/igdb/image/upload/t_cover_big/co6bo0.jpg',
  'Puzzle': 'https://upload.wikimedia.org/wikipedia/en/4/49/Half-Life_Alyx_Cover_Art.jpg',
  'Simulation': 'https://upload.wikimedia.org/wikipedia/en/2/22/Death_Stranding.jpg',
};

@Component({
  selector: 'app-storefront',
  standalone: false,
  templateUrl: './storefront.component.html',
  styleUrl: './storefront.component.scss',
})

export class StorefrontComponent {

  private gamesApi = inject(GamesApiService);
  private genresApi = inject(GenresApiService);
  currentYear: string = "2025";

  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;

  editorsPicks: StorefrontGameDto[] = [];
  recentlyAddedGames: StorefrontGameDto[] = [];
  topRatedWeekGames: StorefrontGameDto[] = [];
  underFiveGames: StorefrontGameDto[] = [];
  freeGames: StorefrontGameDto[] = [];
  genres: GenreDto[] = [];

  ngOnInit(): void {
    this.gamesApi.storefront({
      paging: { page: 1, pageSize: 5 },
      sort: 'topRatedAllTime'
    }).subscribe(res => {
      this.editorsPicks = res.items;
    });

    this.gamesApi.storefront({
      paging: { page: 1, pageSize: 8 },
      sort: 'recentlyAdded'
    }).subscribe(res => {
      this.recentlyAddedGames = res.items;
    });

    this.gamesApi.storefront({
      paging: { page: 1, pageSize: 5 },
      sort: 'topRatedWeek'
    }).subscribe(res => {
      this.topRatedWeekGames = res.items;
    });

    this.gamesApi.storefront({
      paging: { page: 1, pageSize: 12 },
      minPrice: 0.01,
      maxPrice: 4.99
    }).subscribe(res => {
      this.underFiveGames = res.items;
    });

    this.gamesApi.storefront({
      paging: { page: 1, pageSize: 12 },
      maxPrice: 0
    }).subscribe(res => {
      this.freeGames = res.items;
    });

    this.genresApi.list({ paging: { page: 1, pageSize: 1000 } }).subscribe(res => {
      this.genres = res.items ?? [];
    });
  }

  getGameImage(game: StorefrontGameDto): string {
    return game.screenshots?.[0]?.imageURL ?? game.coverImageURL ?? '/carousel-placeholder-image.png';
  }

  recentlyAddedPage = 0;
  editorsPicksPage = 0;
  topRatedWeekPage = 0;
  underFivePage = 0;
  freePage = 0;
  categoriesPage = 0;

  pageItems<T>(items: T[], page: number, perPage: number): T[] {
    return items.slice(page * perPage, page * perPage + perPage);
  }

  getGenreImage(genreName: string): string | null {
    return GENRE_IMAGES[genreName] ?? null;
  }

  heroIndex = 0;

  nextHero(): void {
    if (this.editorsPicks.length === 0) return;
    this.heroIndex = (this.heroIndex + 1) % this.editorsPicks.length;
  }

  prevHero(): void {
    if (this.editorsPicks.length === 0) return;
    this.heroIndex = (this.heroIndex - 1 + this.editorsPicks.length) % this.editorsPicks.length;
  }

  goToHeroSlide(index: number): void {
    this.heroIndex = index;
  }


  private authFacadeService = inject(AuthFacadeService);
  private router = inject(Router);


  logout(): void {
    this.router.navigate(['/auth/logout']);
  }



}
