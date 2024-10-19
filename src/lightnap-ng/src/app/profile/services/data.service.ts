import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import {
    API_URL_ROOT,
    ApiResponse,
} from "@core";
import { ChangePasswordRequest, Device, Profile, UpdateProfileRequest } from "@profile";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrl = inject(API_URL_ROOT);

  changePassword(changePasswordRequest: ChangePasswordRequest) {
    return this.#http.post<ApiResponse<string>>(`${this.#apiUrl}profile/change-password`, changePasswordRequest);
  }

  getProfile() {
    return this.#http.get<ApiResponse<Profile>>(`${this.#apiUrl}profile`);
  }

  updateProfile(updateProfile: UpdateProfileRequest) {
    return this.#http.post<ApiResponse<Profile>>(`${this.#apiUrl}profile`, updateProfile);
  }

  getDevices() {
    return this.#http.get<ApiResponse<Device>>(`${this.#apiUrl}devices`);
  }

  revokeDevice(deviceId: string) {
    return this.#http.delete<ApiResponse<boolean>>(`${this.#apiUrl}devices/${deviceId}`);
  }


}
