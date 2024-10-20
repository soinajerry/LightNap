import { Component, inject } from "@angular/core";
import { LayoutService } from "src/app/layout/services/layout.service";
import { MenuService } from "src/app/layout/services/menu.service";
import { AppMenuItemComponent } from "../app-menu-item/app-menu-item.component";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-menu",
  standalone: true,
  templateUrl: "./app-menu.component.html",
  imports: [CommonModule, AppMenuItemComponent],
})
export class AppMenuComponent {
  layoutService = inject(LayoutService);
  #menuService = inject(MenuService);

  menuItems$ = this.#menuService.watchMenuItems$();
}
