import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { ProfileService } from "@profile/services/profile.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  templateUrl: "./devices.component.html",
  imports: [CommonModule, TableModule, ButtonModule, ErrorListComponent, CardModule, ApiResponseComponent],
})
export class DevicesComponent {
  #profileService = inject(ProfileService);

  devices$ = this.#profileService.getDevices();

  errors = new Array<string>();

  revokeDevice(deviceId: string) {
    this.#profileService.revokeDevice(deviceId).subscribe({
      next: response => {
        this.errors = response.errorMessages;

        if (!response.result) {
          return;
        }

        this.devices$ = this.#profileService.getDevices();
      },
    });
  }
}
