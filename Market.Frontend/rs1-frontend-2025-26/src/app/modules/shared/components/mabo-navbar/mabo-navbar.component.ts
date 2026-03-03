import { Component, inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { ToasterService } from '../../../../core/services/toaster.service';

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
  toaster=inject(ToasterService);
  
  logout():void{
    this.router.navigate(['/auth/logout']);
  }
  
  cartbtnclick():void{
    if(!this.isAuthenticated()){
      this.toaster.error("You need to be logged in to access the cart.");
    }
    else{
      this.router.navigate(['/public/cart']);
    }
  }

  adminPanelBtnClick():void{
    if(!this.isAuthenticated()){
      this.toaster.error("You need to be logged in to access the admin panel.");
    }
    else if(!this.isAdmin()){
      this.toaster.error("You do not have permission to access the admin panel.");
    }
    else{
      this.router.navigate(['/admin']);
    }
  }

}
