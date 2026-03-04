import { Component, inject } from '@angular/core';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { StorefrontGameDto } from '../../../api-services/games/games-api.models';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-storefront',
  standalone: false,
  templateUrl: './storefront.component.html',
  styleUrl: './storefront.component.scss',
})

export class StorefrontComponent {
  
  private gamesApi = inject(GamesApiService);
  currentYear: string = "2025";
  
  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;

  featuredGame?:StorefrontGameDto;
  newestGames: StorefrontGameDto[] = [];
  cheapestGames: StorefrontGameDto[] =[];



  ngOnInit(): void {
    this.gamesApi.storefront({
      paging:{page:1, pageSize: 5},
    }).subscribe(res=>{
      this.newestGames = res.items;

      this.featuredGame=res.items[0];


    });
   
    this.gamesApi.storefront({
      paging:{page:1, pageSize:4},
      sort:'priceAsc'
    }).subscribe(res=>{
      this.cheapestGames=res.items;
    })
  }   

  
  getGameImage(game: StorefrontGameDto): string{
    return game.screenshots?.[0]?.imageURL ?? '/carousel-placeholder-image.png';
  }
  
  newestIndex = 1;

  nextNewest():void{
    if(this.newestGames.length===0) return;
    this.newestIndex = (this.newestIndex+1) % this.newestGames.length;
  }

  prevNewest():void{
    if(this.newestGames.length===0)return;
    this.newestIndex = (this.newestIndex-1 + this.newestGames.length) % this.newestGames.length;
  }





  private authFacadeService = inject(AuthFacadeService);
  private router = inject(Router);


  logout():void{
    this.router.navigate(['/auth/logout']);
  }



}
