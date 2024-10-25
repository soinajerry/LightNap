import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { API_URL_ROOT } from "@core";
import { IdentityService } from "src/app/identity/services/identity.service";
import { Observable } from "rxjs";

export function tokenInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const apiUrlRoot = inject(API_URL_ROOT);

  if (request.url.startsWith(apiUrlRoot)) {
    const token = inject(IdentityService).getBearerToken();
    if (token) {
      return next(
        request.clone({
          headers: request.headers.append("Authorization", token),
          withCredentials: true,
        })
      );
    }
  }

  return next(request);
}
