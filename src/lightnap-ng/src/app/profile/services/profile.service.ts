import { inject, Injectable } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { IdentityService } from "@identity";
import { BrowserSettings, ChangePasswordRequest, StyleSettings, UpdateProfileRequest } from "@profile";
import { filter, of, switchMap, tap } from "rxjs";
import { DataService } from "./data.service";
import { ApiResponse, SuccessApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class ProfileService {
  #dataService = inject(DataService);
  #identityService = inject(IdentityService);

  // This should be kept in sync with the server-side BrowserSettings class.
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
    preferences: {},
  };

  #settings?: BrowserSettings;

  constructor() {
    this.#identityService
      .watchLoggedIn$()
      .pipe(
        takeUntilDestroyed(),
        filter(loggedIn => !loggedIn)
      )
      .subscribe(() => {
        this.#settings = undefined;
      });
  }

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
    if (this.#settings) return of(new SuccessApiResponse(this.#settings) as ApiResponse<BrowserSettings>);

    return this.#dataService.getSettings().pipe(
      tap(response => {
        this.#settings = JSON.parse(JSON.stringify(response));
      })
    );
  }

  updateSettings(browserSettings: BrowserSettings) {
    if (this.#settings) {
      this.#settings = browserSettings;
    }
    return this.#dataService.updateSettings(browserSettings);
  }

  updateStyleSettings(styleSettings: StyleSettings) {
    return this.getSettings().pipe(
      switchMap((response) => {
        if (!response.result || JSON.stringify(response.result.style) === JSON.stringify(styleSettings)) return of(response);
        return this.updateSettings({ ...response.result, style: styleSettings });
      })
    );
  }

  getDefaultStyleSettings() {
    return JSON.parse(JSON.stringify(this.#defaultBrowserSettings.style));
  }

  hasLoadedStyleSettings() {
    return !!this.#settings;
  }
}
