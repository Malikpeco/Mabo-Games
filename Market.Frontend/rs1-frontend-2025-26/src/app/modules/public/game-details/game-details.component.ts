import { Location } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { GameDetailsDto } from '../../../api-services/games/games-api.models';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { CartsApiService } from '../../../api-services/carts/carts-api.service';
import { ToasterService } from '../../../core/services/toaster.service';
import { UserGamesApiService } from '../../../api-services/user-games/user-games-api.service';
import { FavouritesApiService } from '../../../api-services/favourites/favourites-api.service';

@Component({
  selector: 'app-game-details',
  standalone: false,
  templateUrl: './game-details.component.html',
  styleUrl: './game-details.component.scss',
})
export class GameDetailsComponent 
implements OnInit{

  router = inject(Router);
  route = inject(ActivatedRoute);
  api=inject(GamesApiService);
  id = 0;
  game:GameDetailsDto | null = null;
  isInCart = false;
  isInLibrary = false;
  isInFavourites = false;
  cartsApi=inject(CartsApiService);
  userGamesApi=inject(UserGamesApiService);
  toaster=inject(ToasterService);
  location = inject(Location);
  favouritesApi = inject(FavouritesApiService);
  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;
  private authFacadeService = inject(AuthFacadeService);
  
  logout():void{
    this.authFacadeService.logout();
    this.router.navigate(['/']);
  }

  goBack(): void {
    if (window.history.length > 1) {
      this.location.back();
      return;
    }

    this.router.navigate(['/public/browse-games']);
  }
  
  
  ngOnInit(): void {
  let idString = this.route.snapshot.paramMap.get('id');
  this.id=Number(idString);
  this.api.getById(this.id).subscribe({
    next:response=>{
      this.game=response;
      this.loadUserGameState(this.id);
    }
  });
  }

  private loadUserGameState(gameId: number): void {
    if (!this.isAuthenticated()) {
      this.isInCart = false;
      this.isInLibrary = false;
      this.isInFavourites = false;
      return;
    }

    this.cartsApi.getCart().subscribe({
      next: response => {
        this.isInCart = response.cartItems.some(item => item.gameId === gameId);
      },
      error: () => {
        this.isInCart = false;
      }
    });

    this.userGamesApi.listUserGames().subscribe({
      next: res => {
        this.isInLibrary = res.items.some(g => g.id === gameId);
      },
      error: () => {
        this.isInLibrary = false;
      }
    });

    this.favouritesApi.listFavouritesQuery().subscribe({
      next:res=>{
        this.isInFavourites=res.items.some(f=>f.id===gameId);
      },
      error: () => {
        this.isInFavourites = false;
      }
    })
  }

  activeScreenshotIndex: number | null = null;

get isViewingScreenshot(): boolean {
  return this.activeScreenshotIndex !== null;
}

get mainImageUrl(): string {
  if (!this.game) return '';

  if (this.isViewingScreenshot) {
    return this.game.screenshots[this.activeScreenshotIndex!].imageURL; // change field if needed
  }

  return this.game.coverImageURL ?? this.game.screenshots[0]?.imageURL ?? '';
}

openScreenshot(index: number): void {
  this.activeScreenshotIndex = index;
}

closeScreenshot(): void {
  this.activeScreenshotIndex = null;
}

nextScreenshot(): void {
  if (!this.game?.screenshots?.length || this.activeScreenshotIndex === null) return;
  const total = this.game.screenshots.length;
  this.activeScreenshotIndex = (this.activeScreenshotIndex + 1) % total;
}

prevScreenshot(): void {
  if (!this.game?.screenshots?.length || this.activeScreenshotIndex === null) return;
  const total = this.game.screenshots.length;
  this.activeScreenshotIndex = (this.activeScreenshotIndex - 1 + total) % total;
}


addToCart(gameId:number) :void{
  if (!this.isAuthenticated()) {
    this.toaster.error('You must be logged in to add a game to your cart.');
    return;
  }

  if (this.isInLibrary) {
    this.toaster.info('You already own this game.');
    return;
  }

  if (this.isInCart) {
    this.toaster.info('Game is already in your cart.');
    return;
  }

  this.cartsApi.getCart().subscribe({
    next:response=>{
      if(response.cartItems.some(item=>item.gameId===gameId)){
        this.toaster.info('Game is already in your cart.');
        return;
      }

      this.userGamesApi.listUserGames().subscribe({
        next:res=>{
          if(res.items.some(g=>g.id===gameId)){
            this.toaster.info('You already own this game.');
            return;
          }

          this.cartsApi.addToCart({gameId}).subscribe({
            next:()=>{
              this.isInCart = true;
              this.toaster.success('Game added to cart!');
            },
            error:(err)=>{
              this.toaster.error(`${err?.error?.message ?? 'Failed to add game to cart.'}`); 
            },
          });
        },
        error:(err)=>{
          this.toaster.error(`${err?.error?.message ?? 'Failed to check owned games.'}`);
        }
      });
    },
    error:(err)=>{
      this.toaster.error(`${err?.error?.message ?? 'Failed to check cart.'}`);
    }
  });
}

addToFavourites(gameId:number):void{
  this.favouritesApi.addToFavourites(gameId).subscribe({
  next:(response)=>{
    this.toaster.success('Game added to favourites!');
    this.isInFavourites=true;
  }    
  })
}

removeFromFavourites(gameId:number):void{
  this.favouritesApi.removeFromFavourites(gameId).subscribe({
    next:()=>{
      this.isInFavourites=false;
    }
  });
}


}
