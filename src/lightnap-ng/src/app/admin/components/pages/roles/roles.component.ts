import { AdminService } from '@admin/services/admin.service';
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { CardModule } from 'primeng/card';

@Component({
    standalone: true,
    templateUrl: './roles.component.html',
    imports: [CommonModule, CardModule]
})
export class RolesComponent {
    #adminService = inject(AdminService);

    roles$ = this.#adminService.getRoles();
}
