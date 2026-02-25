import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GamesApiService } from '../../../api-services/games/games-api.service';
import { GameDetailsDto } from '../../../api-services/games/games-api.models';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { CartsApiService } from '../../../api-services/carts/carts-api.service';
import { ToasterService } from '../../../core/services/toaster.service';

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
  cartsApi=inject(CartsApiService);
  toaster=inject(ToasterService);

  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;
  private authFacadeService = inject(AuthFacadeService);
  
  logout():void{
    this.authFacadeService.logout();
    this.router.navigate(['/']);
  }
  
  
  ngOnInit(): void {
  let idString = this.route.snapshot.paramMap.get('id');
  this.id=Number(idString);
  this.api.getById(this.id).subscribe({
    next:response=>{
      this.game=response;
    }
  });
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

  this.cartsApi.addToCart({gameId}).subscribe({
    next:()=>{
      this.toaster.success('Game added to cart!');
    },
    error:(err)=>{
      this.toaster.error(`${err.error.message}`); 
    },
  });
}

}
