import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    selector: 'app-privacy-policy',
    standalone: true,
    templateUrl: './privacy-policy.component.html',
    imports: [RouterModule]
})
export class PrivacyPolicyComponent {
    layoutService = inject(LayoutService);
}
