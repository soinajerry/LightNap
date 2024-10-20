import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, of, Subject } from "rxjs";
import { MenuChangeEvent } from "../models/menu-change-event";
import { MenuItem } from "primeng/api";
import { ROUTE_HELPER } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

@Injectable({
  providedIn: "root",
})
export class MenuService {
  #routeHelper = inject(ROUTE_HELPER);
  #identityService = inject(IdentityService);

  #menuSource = new Subject<MenuChangeEvent>();
  menuSource$ = this.#menuSource.asObservable();

  #defaultMenuItems = new Array<MenuItem>({
    label: "Home",
    items: [{ label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeHelper.getRoute("home") }],
  });
  #menuItemSubject = new BehaviorSubject<Array<MenuItem>>(this.#defaultMenuItems);

  #isAdminLoggedIn = false;

  constructor() {
    this.#identityService.watchLoggedInToRole$("Administrator").pipe(takeUntilDestroyed()).subscribe((isAdminLoggedIn) => {
      this.#isAdminLoggedIn = isAdminLoggedIn;
      this.#refreshMenuItems();
    });
  }

  onMenuStateChange(event: MenuChangeEvent) {
    this.#menuSource.next(event);
  }

  #refreshMenuItems() {
    var menuItems = [...this.#defaultMenuItems];

    if (this.#isAdminLoggedIn) {
      menuItems.push({
        label: "Admin",
        items: [
          { label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeHelper.getRoute("admin") },
          { label: "Users", icon: "pi pi-fw pi-users", routerLink: this.#routeHelper.getRoute("admin-users") },
          { label: "Roles", icon: "pi pi-fw pi-lock", routerLink: this.#routeHelper.getRoute("admin-roles") },
        ],
      });
    }

    this.#menuItemSubject.next(menuItems);
  }

  watchMenuItems$() {
    return this.#menuItemSubject.asObservable();
  }
}
