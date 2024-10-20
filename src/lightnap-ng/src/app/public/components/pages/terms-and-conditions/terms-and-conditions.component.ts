import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    selector: 'app-terms-and-conditions',
    standalone: true,
    templateUrl: './terms-and-conditions.component.html',
    imports: [RouterModule]
})
export class TermsAndConditionsComponent {
    layoutService = inject(LayoutService);
}
