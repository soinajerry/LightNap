import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ROUTE_HELPER } from '@core';
import { IdentityService } from '@core/services/identity.service';
import { ProfileService } from '@profile/services/profile.service';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { tap } from 'rxjs';

@Component({
    selector: 'app-profile-index',
    standalone: true,
    templateUrl: './index.component.html',
    imports: [CommonModule, TableModule, ButtonModule]
})
export class IndexComponent {
    #identityService = inject(IdentityService);
    #profileService = inject(ProfileService);
    #routeHelper = inject(ROUTE_HELPER);

    profile$ = this.#profileService.getProfile();
    devices$ = this.#profileService.getDevices();

    revokeDevice(deviceId: string) {
        this.#profileService.revokeDevice(deviceId).subscribe({
            next: (response => {
                if (!response.result) {
                    alert("An error occurred while trying to remove the device");
                    return;
                }

                this.devices$ = this.#profileService.getDevices().pipe(tap(r => console.log(r)));
            })
        })
    }

    logOut() {
        this.#identityService.logOut().subscribe({
            next: response => {
                if (response.result) {
                    this.#routeHelper.navigate("landing");
                }
            }
        })
    }
}
