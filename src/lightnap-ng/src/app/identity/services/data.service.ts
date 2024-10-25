import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import {
    API_URL_ROOT,
    ApiResponse,
    LoginRequest,
    LoginResult,
    NewPasswordRequest,
    ResetPasswordRequest,
    VerifyCodeRequest,
} from "@core";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #identityApiUrlRoot = `${inject(API_URL_ROOT)}identity/`;

  getAccessToken() {
    return this.#http.get<ApiResponse<string>>(`${this.#identityApiUrlRoot}access-token`);
  }

  logIn(loginRequest: LoginRequest) {
    return this.#http.post<ApiResponse<LoginResult>>(`${this.#identityApiUrlRoot}login`, loginRequest);
  }

  register(registerRequest: LoginRequest) {
    return this.#http.post<ApiResponse<LoginResult>>(`${this.#identityApiUrlRoot}register`, registerRequest);
  }

  logOut() {
    return this.#http.get<ApiResponse<boolean>>(`${this.#identityApiUrlRoot}logout`);
  }

  resetPassword(resetPasswordRequest: ResetPasswordRequest) {
    return this.#http.post<ApiResponse<boolean>>(`${this.#identityApiUrlRoot}reset-password`, resetPasswordRequest);
  }

  newPassword(newPasswordRequest: NewPasswordRequest) {
    return this.#http.post<ApiResponse<string>>(`${this.#identityApiUrlRoot}new-password`, newPasswordRequest);
  }

  verifyCode(verifyCodeRequest: VerifyCodeRequest) {
    return this.#http.post<ApiResponse<string>>(`${this.#identityApiUrlRoot}verify-code`, verifyCodeRequest);
  }
}
