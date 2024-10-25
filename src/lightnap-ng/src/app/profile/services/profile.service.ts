import { inject, Injectable } from "@angular/core";
import { BrowserSettings, ChangePasswordRequest, StyleSettings, UpdateProfileRequest } from "@profile";
import { DataService } from "./data.service";
import { delay, map, of, switchMap, tap } from "rxjs";
import { ApiResponse, ErrorApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class ProfileService {
  #dataService = inject(DataService);

  #defaultBrowserSettings: BrowserSettings = {
    style: {
      ripple: true,
      inputStyle: "outlined",
      menuMode: "static",
      colorScheme: "light",
      theme: "lara-light-indigo",
      scale: 14,
    },
    extended: {},
    features: {},
    preferences: {}
  };

  #settingsResponse?: ApiResponse<BrowserSettings>;

  getProfile() {
    return this.#dataService.getProfile();
  }

  updateProfile(updateProfileRequest: UpdateProfileRequest) {
    return this.#dataService.updateProfile(updateProfileRequest);
  }

  getDevices() {
    return this.#dataService.getDevices();
  }

  revokeDevice(deviceId: string) {
    return this.#dataService.revokeDevice(deviceId);
  }

  changePassword(changePasswordRequest: ChangePasswordRequest) {
    return this.#dataService.changePassword(changePasswordRequest);
  }

  getSettings() {
    if (this.#settingsResponse) return of(this.#settingsResponse);

    return this.#dataService.getSettings().pipe(
      tap(response => {
        if (response.result) {
          this.#settingsResponse = response;
          this.#settingsResponse.result = { ...this.#defaultBrowserSettings, ...response.result };
        }
      })
    );
  }

  updateSettings(browserSettings: BrowserSettings) {
    if (this.#settingsResponse) {
      this.#settingsResponse.result = browserSettings;
    }
    return this.#dataService.updateSettings(browserSettings);
  }

  updateStyleSettings(styleSettings: StyleSettings) {
    return this.#dataService.getSettings().pipe(
      switchMap(response => {
        if (!response.result || JSON.stringify(response.result.style) === JSON.stringify(styleSettings)) return of(response);
        return this.updateSettings({ ...response.result, style: styleSettings });
      })
    );
  }

  hasLoadedStyleSettings() {
    return !!this.#settingsResponse;
  }
}
