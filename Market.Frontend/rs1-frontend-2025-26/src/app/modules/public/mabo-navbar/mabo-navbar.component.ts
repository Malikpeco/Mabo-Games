import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

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
  
  logout():void{
    this.authFacadeService.logout();
    this.router.navigate(['/']);
  }
}
