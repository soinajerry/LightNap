import { Component, inject, Input } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormsModule } from "@angular/forms";
import { IdentityService } from "@core/services/identity.service";
import { BrowserSettings } from "@profile";
import { ProfileService } from "@profile/services/profile.service";
import { SharedModule } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { InputSwitchModule } from "primeng/inputswitch";
import { RadioButtonModule } from "primeng/radiobutton";
import { SidebarModule } from "primeng/sidebar";
import { LayoutService } from "src/app/layout/services/layout.service";
import { MenuService } from "src/app/layout/services/menu.service";

@Component({
  selector: "app-config",
  standalone: true,
  templateUrl: "./app-config.component.html",
  imports: [SharedModule, FormsModule, SidebarModule, ButtonModule, RadioButtonModule, InputSwitchModule],
})
export class AppConfigComponent {
  @Input() minimal: boolean = false;
  layoutService = inject(LayoutService);
  menuService = inject(MenuService);
  identityService = inject(IdentityService);
  profileService = inject(ProfileService);

  #settings?: BrowserSettings;

  scales: number[] = [12, 13, 14, 15, 16];

  constructor() {
    this.identityService
      .watchLoggedIn$()
      .pipe(takeUntilDestroyed())
      .subscribe(loggedIn => {
        if (loggedIn) {
          this.profileService.getSettings().subscribe(response => {
            if (response.result) {
              this.#settings = response.result;
              this.#updateTheme(response.result.theme);
            }
          });
        } else {
          this.#settings = undefined;
        }
      });
  }

  get visible(): boolean {
    return this.layoutService.state.configSidebarVisible;
  }
  set visible(_val: boolean) {
    this.layoutService.state.configSidebarVisible = _val;
  }

  get scale(): number {
    return this.layoutService.config().scale;
  }
  set scale(_val: number) {
    this.layoutService.config.update(config => ({
      ...config,
      scale: _val,
    }));
  }

  get menuMode(): string {
    return this.layoutService.config().menuMode;
  }
  set menuMode(_val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      menuMode: _val,
    }));
  }

  get inputStyle(): string {
    return this.layoutService.config().inputStyle;
  }
  set inputStyle(_val: string) {
    this.layoutService.config().inputStyle = _val;
  }

  get ripple(): boolean {
    return this.layoutService.config().ripple;
  }
  set ripple(_val: boolean) {
    this.layoutService.config.update(config => ({
      ...config,
      ripple: _val,
    }));
  }

  #updateTheme(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      theme: val,
    }));
  }

  set theme(val: string) {
    this.#updateTheme(val);

    if (this.#settings) {
      this.#settings.theme = val;
      this.profileService.updateSettings(this.#settings).subscribe();
    }
  }
  get theme(): string {
    return this.layoutService.config().theme;
  }

  set colorScheme(val: string) {
    this.layoutService.config.update(config => ({
      ...config,
      colorScheme: val,
    }));
  }
  get colorScheme(): string {
    return this.layoutService.config().colorScheme;
  }

  onConfigButtonClick() {
    this.layoutService.showConfigSidebar();
  }

  changeTheme(theme: string, colorScheme: string) {
    this.theme = theme;
    this.colorScheme = colorScheme;
  }

  decrementScale() {
    this.scale--;
  }

  incrementScale() {
    this.scale++;
  }
}
