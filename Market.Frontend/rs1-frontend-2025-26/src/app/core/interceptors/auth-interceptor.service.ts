import {
    HttpInterceptorFn,
    HttpErrorResponse,
    HttpRequest,
    HttpHandlerFn
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AuthFacadeService } from '../services/auth/auth-facade.service';

// Global state for refresh (shared between requests)
let refreshInProgress = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

/**
 * HTTP interceptor that:
 * 1. Adds Authorization header with access token
 * 2. Handles 401 errors by refreshing token
 * 3. Retries failed request with new token
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const auth = inject(AuthFacadeService);

    // 1) Skip auth endpoints (login/refresh/logout)
    if (isAuthEndpoint(req.url)) {
        return next(req);
    }

    // 2) Add Authorization header if token exists
    const accessToken = auth.getAccessToken();
    let authReq = req;

    if (accessToken) {
        authReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        });
    }

    // 3) Handle 401 → refresh → retry
    return next(authReq).pipe(
        catchError((err) => {
            if (err instanceof HttpErrorResponse && err.status === 401) {
                return handle401Error(authReq, next, auth);
            }
            return throwError(() => err);
        })
    );
};

/**
 * Check if URL is an auth endpoint that should not be intercepted.
 */
function isAuthEndpoint(url: string): boolean {
    return url.includes('/Auth/');
}

/**
 * Handle 401 error by refreshing token and retrying request.
 */
function handle401Error(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
    auth: AuthFacadeService
): Observable<any> {
    const refreshToken = auth.getRefreshToken();

    // No refresh token → redirect to login
    if (!refreshToken) {
        auth.redirectToLogin();
        return throwError(() => new Error('No refresh token'));
    }

    // If refresh already in progress → wait for result
    if (refreshInProgress) {
        return refreshTokenSubject.pipe(
            filter((token) => token !== null),
            take(1),
            switchMap((token) => {
                const cloned = token
                    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
                    : req;
                return next(cloned);
            })
        );
    }

    // Start refresh process
    refreshInProgress = true;
    refreshTokenSubject.next(null);

    return auth.refresh({ refreshToken, fingerprint: null }).pipe(
        switchMap((res) => {
            refreshInProgress = false;
            const newAccessToken = res.accessToken;
            refreshTokenSubject.next(newAccessToken);

            // Retry original request with new token
            const clonedReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newAccessToken}` }
            });

            return next(clonedReq);
        }),
        catchError((error) => {
            refreshInProgress = false;
            refreshTokenSubject.next(null);
            auth.redirectToLogin();
            return throwError(() => error);
        })
    );
}
