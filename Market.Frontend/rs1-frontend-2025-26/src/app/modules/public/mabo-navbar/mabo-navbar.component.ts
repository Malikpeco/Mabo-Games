import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { ToasterService } from '../../../core/services/toaster.service';

@Component({
  selector: 'app-mabo-navbar',
  standalone: false,
  templateUrl: './mabo-navbar.component.html',
  styleUrl: './mabo-navbar.component.scss',
})
export class MaboNavbarComponent {
  router = inject(Router);
  route = inject(ActivatedRoute);
  private currentUserService = inject(CurrentUserService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;
  private authFacadeService = inject(AuthFacadeService);
  toaster=inject(ToasterService);
  
  logout():void{
    this.authFacadeService.logout();
    this.router.navigate(['/']);
  }

  cartbtnclick():void{
    if(!this.isAuthenticated()){
      this.toaster.error("You need to be logged in to access the cart.");
    }
    else{
      this.router.navigate(['/public/cart']);
    }
  }

  isNavActive(key: 'cart'): boolean {
    const url = this.router.url;
    return url.startsWith('/public/cart') || url.startsWith('/public/checkout') || url.startsWith('/public/payment');
  }
}
