import { inject, OnInit } from "@angular/core";
import { Component } from "@angular/core";
import { LayoutService } from "./service/app.layout.service";
import { AppMenuitemComponent } from "./app.menuitem.component";
import { MenuItem } from "primeng/api";
import { ROUTE_HELPER } from "@core";

@Component({
  selector: "app-menu",
  standalone: true,
  templateUrl: "./app.menu.component.html",
  imports: [AppMenuitemComponent],
})
export class AppMenuComponent implements OnInit {
  #routeHelper = inject(ROUTE_HELPER);

  model: MenuItem[] = [];

  constructor(public layoutService: LayoutService) {}

  ngOnInit() {
    this.model = [
      {
        label: "Home",
        items: [{ label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeHelper.getRoute("home") }],
      },
    ];
  }
}
