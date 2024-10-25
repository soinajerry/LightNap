import { inject, Injectable } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { JwtHelperService } from "@auth0/angular-jwt";
import { LoginRequest, NewPasswordRequest, RegisterRequest, ResetPasswordRequest, VerifyCodeRequest } from "@core";
import { distinctUntilChanged, filter, map, ReplaySubject, take, tap } from "rxjs";
import { DataService } from "./data.service";
import { TimerService } from "../../core/services/timer.service";

@Injectable({
  providedIn: "root",
})
export class IdentityService {
  // How often we check if we need to refresh the token. (Evaluate the expiration every minute.)
  static readonly TokenRefreshCheckMillis = 60 * 1000;
  // How close to expiration we should try to refresh the token. (Refresh if it expires in less than 5 minutes.)
  static readonly TokenExpirationWindowMillis = 5 * 60 * 1000;

  #timer = inject(TimerService);
  #dataService = inject(DataService);

  #token?: string;
  #expires = 0;

  #loggedInBehaviorSubject$ = new ReplaySubject<boolean>(1);
  #loggedInRolesSubject$ = new ReplaySubject<Array<string>>(1);
  #requestingRefreshToken = false;

  get loggedIn() { return !!this.#token; }

  constructor() {
    this.#timer
      .watchTimer$(IdentityService.TokenRefreshCheckMillis)
      .pipe(
        takeUntilDestroyed(),
        filter(
          () => !this.#requestingRefreshToken && this.#token?.length > 0 && this.#expires + IdentityService.TokenExpirationWindowMillis < Date.now()
        )
      )
      .subscribe({
        next: () => {
          this.#tryRefreshToken();
        },
      });

    // BUG: We need to request an access token from the server as soon as we load. However, we can't do it while the constructor is loading or
    // else we run into a circular DI issue with the DataService's HttpClient and its HttpInterceptors that reference this service. To work
    // around this we sleep in the hopes everything will be loaded by the time we make the request. However, this doesn't seem guaranteed
    // and it was observed during debugging that a race condition still exists. The only other solution is to set up an app initializer that
    // loads and calls an explicit refreshToken() method we could expose from this service, but that needs to be evaluated when the time comes.
    this.#timer
      .watchTimer$(100)
      .pipe(take(1))
      .subscribe(() => this.#tryRefreshToken());
  }

  #tryRefreshToken() {
    if (this.#requestingRefreshToken) return;
    this.#requestingRefreshToken = true;
    this.#dataService.getAccessToken().subscribe({
      next: response => {
        this.#onTokenReceived(response.result);
      },
      complete: () => (this.#requestingRefreshToken = false),
    });
  }

  #onTokenReceived(token?: string) {
    this.#token = token;
    this.#loggedInBehaviorSubject$.next(!!this.#token);

    if (this.#token) {
      const helper = new JwtHelperService();
      const decodedToken = helper.decodeToken(this.#token);
      this.#expires = decodedToken.exp * 1000;
      const roles = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      if (!roles) {
        this.#loggedInRolesSubject$.next([]);
      } else if (Array.isArray(roles)) {
        this.#loggedInRolesSubject$.next(roles);
      } else {
        this.#loggedInRolesSubject$.next([roles]);
      }
    } else {
      this.#expires = 0;
      this.#loggedInRolesSubject$.next([]);
    }
  }

  watchLoggedIn$() {
    return this.#loggedInBehaviorSubject$.pipe(distinctUntilChanged());
  }

  watchLoggedInToRole$(role: string) {
    return this.#loggedInRolesSubject$.pipe(
      map(roles => {
        return !!roles?.find(roleItem => roleItem === role);
      })
    );
  }

  getBearerToken() {
    return `Bearer ${this.#token}`;
  }

  logIn(loginRequest: LoginRequest) {
    return this.#dataService.logIn(loginRequest).pipe(
      tap(response => {
        this.#onTokenReceived(response.result?.bearerToken);
      })
    );
  }

  register(registerRequest: RegisterRequest) {
    return this.#dataService.register(registerRequest).pipe(
      tap(response => {
        this.#onTokenReceived(response.result?.bearerToken);
      })
    );
  }

  logOut() {
    return this.#dataService.logOut().pipe(
      tap(response => {
        if (response.result) {
          this.#onTokenReceived(undefined);
        }
      })
    );
  }

  verifyCode(verifyCodeRequest: VerifyCodeRequest) {
    return this.#dataService.verifyCode(verifyCodeRequest).pipe(
      tap(response => {
        this.#onTokenReceived(response.result);
      })
    );
  }

  resetPassword(resetPasswordRequest: ResetPasswordRequest) {
    return this.#dataService.resetPassword(resetPasswordRequest);
  }

  newPassword(newPasswordRequest: NewPasswordRequest) {
    return this.#dataService.newPassword(newPasswordRequest).pipe(
      tap(response => {
        this.#onTokenReceived(response.result);
      })
    );
  }
}
