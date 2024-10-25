import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, combineLatest, debounceTime, of, startWith, Subject, tap } from "rxjs";
import { MenuChangeEvent } from "../models/menu-change-event";
import { MenuItem } from "primeng/api";
import { ROUTE_HELPER } from "@core";
import { IdentityService } from "src/app/identity/services/identity.service";
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

  #loggedInMenuItems = new Array<MenuItem>({
    label: "Profile",
    items: [
      { label: "Profile", icon: "pi pi-fw pi-user", routerLink: this.#routeHelper.getRoute("profile") },
      { label: "Devices", icon: "pi pi-fw pi-mobile", routerLink: this.#routeHelper.getRoute("devices") },
      { label: "Change Password", icon: "pi pi-fw pi-lock", routerLink: this.#routeHelper.getRoute("change-password") },
    ],
  });

  #adminMenuItems = new Array<MenuItem>({
    label: "Admin",
    items: [
      { label: "Home", icon: "pi pi-fw pi-home", routerLink: this.#routeHelper.getRoute("admin") },
      { label: "Users", icon: "pi pi-fw pi-users", routerLink: this.#routeHelper.getRoute("admin-users") },
      { label: "Roles", icon: "pi pi-fw pi-lock", routerLink: this.#routeHelper.getRoute("admin-roles") },
    ],
  });

  #menuItemSubject = new BehaviorSubject<Array<MenuItem>>(this.#defaultMenuItems);

  #isLoggedIn = false;
  #isAdminLoggedIn = false;

  constructor() {
    combineLatest([
      this.#identityService.watchLoggedIn$().pipe(tap(isLoggedIn => (this.#isLoggedIn = isLoggedIn))),
      this.#identityService.watchLoggedInToRole$("Administrator").pipe(tap(isAdminLoggedIn => (this.#isAdminLoggedIn = isAdminLoggedIn))),
    ])
      .pipe(takeUntilDestroyed(), debounceTime(100))
      .subscribe(() => this.#refreshMenuItems());
  }

  onMenuStateChange(event: MenuChangeEvent) {
    this.#menuSource.next(event);
  }

  #refreshMenuItems() {
    var menuItems = [...this.#defaultMenuItems];

    if (this.#isLoggedIn) {
      menuItems.push(...this.#loggedInMenuItems);
    }

    if (this.#isAdminLoggedIn) {
      menuItems.push(...this.#adminMenuItems);
    }

    this.#menuItemSubject.next(menuItems);
  }

  watchMenuItems$() {
    return this.#menuItemSubject.asObservable();
  }
}
