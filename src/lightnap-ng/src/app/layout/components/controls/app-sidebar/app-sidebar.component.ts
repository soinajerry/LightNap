import { Component, ElementRef, inject } from '@angular/core';
import { AppMenuComponent } from '../app-menu/app-menu.component';
import { LayoutService } from 'src/app/layout/services/layout.service';

@Component({
    selector: 'app-sidebar',
    standalone: true,
    templateUrl: './app-sidebar.component.html',
    imports: [AppMenuComponent]
})
export class AppSidebarComponent {
    layoutService = inject(LayoutService);
    el = inject(ElementRef);
}

