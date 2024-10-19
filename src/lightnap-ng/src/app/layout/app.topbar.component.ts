import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { RoutePipe } from '@core';

@Component({
    selector: 'app-topbar',
    standalone: true,
    templateUrl: './app.topbar.component.html',
    imports: [CommonModule, RouterModule, ButtonModule, RippleModule, RoutePipe]
})
export class AppTopBarComponent {
    layoutService = inject(LayoutService);

    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;
}
