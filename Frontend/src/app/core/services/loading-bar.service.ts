import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

/**
 * Service to manage global loading bar state
 *
 * This service tracks the number of active HTTP requests and shows/hides
 * the loading bar accordingly. It uses a counter to handle multiple
 * simultaneous requests.
 *
 * Usage:
 * - Automatically used by loadingBarInterceptorService
 * - Can be manually controlled if needed:
 *   loadingBar.show()  // Show loading bar
 *   loadingBar.hide()  // Hide loading bar
 */
@Injectable({
  providedIn: 'root'
})
export class LoadingBarService {
  /**
   * Counter for active HTTP requests
   * When counter > 0, loading bar is visible
   */
  private activeRequests = 0;

  /**
   * BehaviorSubject to track loading state
   * Components can subscribe to this to react to loading state changes
   */
  private loadingSubject = new BehaviorSubject<boolean>(false);

  /**
   * Public observable that components can subscribe to
   * Emits true when loading starts, false when loading stops
   */
  public loading$ = this.loadingSubject.asObservable();

  /**
   * Increment active requests counter and show loading bar
   *
   * Called when an HTTP request starts
   * If this is the first active request, shows the loading bar
   */
  show(): void {
    this.activeRequests++;

    if (this.activeRequests === 1) {
      // Only show loading bar when transitioning from 0 to 1 active requests
      // This prevents flickering when multiple requests are active
      this.loadingSubject.next(true);
    }
  }

  /**
   * Decrement active requests counter and hide loading bar when all complete
   *
   * Called when an HTTP request completes (success or error)
   * Only hides loading bar when all active requests have completed
   */
  hide(): void {
    this.activeRequests--;

    if (this.activeRequests <= 0) {
      // All requests complete, hide loading bar
      this.activeRequests = 0; // Ensure counter doesn't go negative
      this.loadingSubject.next(false);
    }
  }

  /**
   * Force hide loading bar and reset counter
   *
   * Useful for error recovery or manual control
   * Use with caution as it may hide loading bar while requests are still active
   */
  forceHide(): void {
    this.activeRequests = 0;
    this.loadingSubject.next(false);
  }

  /**
   * Check if currently loading
   *
   * @returns true if loading bar is visible
   */
  isLoading(): boolean {
    return this.loadingSubject.value;
  }

  /**
   * Get current number of active requests
   *
   * Useful for debugging
   * @returns number of active HTTP requests
   */
  getActiveRequestsCount(): number {
    return this.activeRequests;
  }
}
