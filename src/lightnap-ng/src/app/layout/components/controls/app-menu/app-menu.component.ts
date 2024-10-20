import { inject, OnInit } from "@angular/core";
import { Component } from "@angular/core";
import { MenuItem } from "primeng/api";
import { ROUTE_HELPER } from "@core";
import { LayoutService } from "src/app/layout/services/layout.service";
import { AppMenuItemComponent } from "../app-menu-item/app-menu-item.component";

@Component({
  selector: "app-menu",
  standalone: true,
  templateUrl: "./app-menu.component.html",
  imports: [AppMenuItemComponent],
})
export class AppMenuComponent implements OnInit {
  #routeHelper = inject(ROUTE_HELPER);
  layoutService = inject(LayoutService);

  model: MenuItem[] = [];

  ngOnInit() {
    this.model = [
      {
        label: "Home",
        items: [{ label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeHelper.getRoute("home") }],
      },
    ];
  }
}
