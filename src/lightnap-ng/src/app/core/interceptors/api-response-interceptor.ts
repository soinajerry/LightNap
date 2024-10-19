import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpRequest, HttpResponse } from "@angular/common/http";
import { inject } from "@angular/core";
import { ROUTE_HELPER, HttpErrorApiResponse } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { Observable, catchError, of } from "rxjs";
import { environment } from "src/environments/environment";

export function apiResponseInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  return next(request).pipe(
    catchError(error => {
      if (error.status === 401) {
        inject(IdentityService).logOut();
        inject(ROUTE_HELPER).navigate("login");
      }

      if (!environment.production) {
        console.error(error);
      }

      return of(new HttpResponse({ body: new HttpErrorApiResponse(error as HttpErrorResponse), status: error.status }));
    })
  );
}
