import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    standalone: true,
    templateUrl: './privacy-policy.component.html',
    imports: [RouterModule]
})
export class PrivacyPolicyComponent {
    layoutService = inject(LayoutService);
}
