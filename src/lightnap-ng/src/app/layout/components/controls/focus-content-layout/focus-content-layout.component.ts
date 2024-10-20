import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    selector: 'focus-content-layout',
    standalone: true,
    templateUrl: './focus-content-layout.component.html',
    imports: [CommonModule]
})
export class FocusContentLayout {
    layoutService = inject(LayoutService);
}
