// src/app/core/services/auth/current-user.service.ts
import { Injectable, inject, computed } from '@angular/core';
import { AuthFacadeService } from './auth-facade.service';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private auth = inject(AuthFacadeService);

  /** Signal koji UI može čitati (readonly) */
  currentUser = computed(() => this.auth.currentUser());

  isAuthenticated = computed(() => this.auth.isAuthenticated());
  isAdmin = computed(() => this.auth.isAdmin());
  isManager = computed(() => this.auth.isManager());
  isEmployee = computed(() => this.auth.isEmployee());

  get snapshot() {
    return this.auth.currentUser();
  }

  /** Pravilo: admin > ostali → client */
  getDefaultRoute(): string {
    const user = this.snapshot;
    if (!user) return '/login';

    //if (user.isAdmin) return '/admin';
    return '/';
  }
}
