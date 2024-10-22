import { inject, Injectable } from "@angular/core";
import { ChangePasswordRequest, UpdateProfileRequest } from "@profile";
import { DataService } from "./data.service";
import { delay, map } from "rxjs";
import { ErrorApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class ProfileService {
  #dataService = inject(DataService);

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
}
