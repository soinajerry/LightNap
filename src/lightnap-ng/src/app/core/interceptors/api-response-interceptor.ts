import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest, HttpResponse } from "@angular/common/http";
import { inject } from "@angular/core";
import { ROUTE_HELPER, HttpErrorApiResponse } from "@core";
import { IdentityService } from "src/app/identity/services/identity.service";
import { Observable, catchError, of } from "rxjs";
import { environment } from "src/environments/environment";

export function apiResponseInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const identityService = inject(IdentityService);
  const routeHelper = inject(ROUTE_HELPER);

  return next(request).pipe(
    catchError(error => {
      if (error.status === 401) {
        identityService.logOut();
        routeHelper.navigate("login");
      }

      if (!environment.production) {
        console.error(error);
      }

      return of(new HttpResponse({ body: new HttpErrorApiResponse(error as HttpErrorResponse), status: error.status }));
    })
  );
}
