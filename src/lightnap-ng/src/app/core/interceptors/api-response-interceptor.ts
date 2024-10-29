import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest, HttpResponse } from "@angular/common/http";
import { inject } from "@angular/core";
import { HttpErrorApiResponse } from "@core";
import { IdentityService } from "src/app/identity/services/identity.service";
import { Observable, catchError, of } from "rxjs";
import { environment } from "src/environments/environment";
import { RouteAliasService } from "@routing";

/**
 * Intercepts HTTP responses to handle errors and perform specific actions based on the error status.
 *
 * @param request - The outgoing HTTP request.
 * @param next - The next handler in the HTTP request pipeline.
 * @returns An observable of the HTTP event.
 *
 * @remarks
 * - If the response status is 401 (Unauthorized), the user is logged out and navigated to the login page.
 * - In non-production environments, the error is logged to the console.
 * - The function returns an observable of an `HttpResponse` containing an `HttpErrorApiResponse` object.
 */
export function apiResponseInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const identityService = inject(IdentityService);
  const routeAliasService = inject(RouteAliasService);

  return next(request).pipe(
    catchError(error => {
      if (error.status === 401) {
        identityService.logOut();
        routeAliasService.navigate("login");
      }

      if (!environment.production) {
        console.error(error);
      }

      return of(new HttpResponse({ body: new HttpErrorApiResponse(error as HttpErrorResponse), status: error.status }));
    })
  );
}
