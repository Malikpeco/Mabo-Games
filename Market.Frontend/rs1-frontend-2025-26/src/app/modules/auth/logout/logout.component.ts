import { Component, OnInit, inject } from '@angular/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.component.html',  // Keep your existing HTML file
})
export class LogoutComponent implements OnInit {
  private auth = inject(AuthFacadeService);

  ngOnInit(): void {
    this.auth.logout().subscribe();
  }
}