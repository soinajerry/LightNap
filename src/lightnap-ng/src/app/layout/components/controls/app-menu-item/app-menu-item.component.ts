import { animate, state, style, transition, trigger } from "@angular/animations";
import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, HostBinding, inject, Input, OnInit } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { NavigationEnd, Router, RouterLink } from "@angular/router";
import { RippleModule } from "primeng/ripple";
import { filter } from "rxjs/operators";
import { LayoutService } from "src/app/layout/services/layout.service";
import { MenuService } from "src/app/layout/services/menu.service";

@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: "[app-menu-item]",
  standalone: true,
  templateUrl: "./app-menu-item.component.html",
  animations: [
    trigger("children", [
      state(
        "collapsed",
        style({
          height: "0",
        })
      ),
      state(
        "expanded",
        style({
          height: "*",
        })
      ),
      transition("collapsed <=> expanded", animate("400ms cubic-bezier(0.86, 0, 0.07, 1)")),
    ]),
  ],
  imports: [CommonModule, RouterLink, RippleModule],
})
export class AppMenuItemComponent implements OnInit {
  #cd = inject(ChangeDetectorRef);
  #menuService = inject(MenuService);
  layoutService = inject(LayoutService);
  router = inject(Router);

  @Input() item: any;
  @Input() index!: number;
  @Input() @HostBinding("class.layout-root-menuitem") root!: boolean;
  @Input() parentKey!: string;

  active = false;
  key: string = "";

  constructor() {
    this.#menuService.menuSource$.pipe(takeUntilDestroyed()).subscribe(value => {
      Promise.resolve(null).then(() => {
        if (value.routeEvent) {
          this.active = value.key === this.key || value.key.startsWith(this.key + "-") ? true : false;
        } else {
          if (value.key !== this.key && !value.key.startsWith(this.key + "-")) {
            this.active = false;
          }
        }
      });
    });

    this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(params => {
      if (this.item.routerLink) {
        this.updateActiveStateFromRoute();
      }
    });
  }

  ngOnInit() {
    this.key = this.parentKey ? this.parentKey + "-" + this.index : String(this.index);

    if (this.item.routerLink) {
      this.updateActiveStateFromRoute();
    }
  }

  updateActiveStateFromRoute() {
    let activeRoute = this.router.isActive(this.item.routerLink[0], {
      paths: "exact",
      queryParams: "ignored",
      matrixParams: "ignored",
      fragment: "ignored",
    });

    if (activeRoute) {
      this.#menuService.onMenuStateChange({ key: this.key, routeEvent: true });
    }
  }

  itemClick(event: Event) {
    // avoid processing disabled items
    if (this.item.disabled) {
      event.preventDefault();
      return;
    }

    // execute command
    if (this.item.command) {
      this.item.command({ originalEvent: event, item: this.item });
    }

    // toggle active state
    if (this.item.items) {
      this.active = !this.active;
    }

    this.#menuService.onMenuStateChange({ key: this.key });
  }

  get submenuAnimation() {
    return this.root ? "expanded" : this.active ? "expanded" : "collapsed";
  }

  @HostBinding("class.active-menuitem")
  get activeClass() {
    return this.active && !this.root;
  }
}
