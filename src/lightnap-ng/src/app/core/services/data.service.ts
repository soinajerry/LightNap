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
  #apiUrl = inject(API_URL_ROOT);

  refreshToken() {
    return this.#http.get<ApiResponse<string>>(`${this.#apiUrl}identity/refresh-token`);
  }

  logIn(loginRequest: LoginRequest) {
    return this.#http.post<ApiResponse<LoginResult>>(`${this.#apiUrl}identity/login`, loginRequest);
  }

  register(registerRequest: LoginRequest) {
    return this.#http.post<ApiResponse<LoginResult>>(`${this.#apiUrl}identity/register`, registerRequest);
  }

  logOut() {
    return this.#http.get<ApiResponse<boolean>>(`${this.#apiUrl}identity/logout`);
  }

  resetPassword(resetPasswordRequest: ResetPasswordRequest) {
    return this.#http.post<ApiResponse<boolean>>(`${this.#apiUrl}identity/reset-password`, resetPasswordRequest);
  }

  newPassword(newPasswordRequest: NewPasswordRequest) {
    return this.#http.post<ApiResponse<string>>(`${this.#apiUrl}identity/new-password`, newPasswordRequest);
  }

  verifyCode(verifyCodeRequest: VerifyCodeRequest) {
    return this.#http.post<ApiResponse<string>>(`${this.#apiUrl}identity/verify-code`, verifyCodeRequest);
  }
}
