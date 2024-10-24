import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { ToastModule } from "primeng/toast";
import { provideAnimations } from "@angular/platform-browser/animations";
import { environment } from "src/environments/environment";
import { AppRouteHelper } from "./app-route-helper";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { API_URL_ROOT, ROUTE_HELPER } from "@core";
import { apiResponseInterceptor } from "@core/interceptors/api-response-interceptor";
import { tokenInterceptor } from "@core/interceptors/token-interceptor";
import { MessageService } from "primeng/api";

@NgModule({
  declarations: [AppComponent],
  imports: [AppRoutingModule, BrowserModule, ToastModule],
  providers: [
    provideAnimations(),
    { provide: API_URL_ROOT, useValue: environment.apiUrlRoot },
    { provide: ROUTE_HELPER, useClass: AppRouteHelper },
    { provide: LocationStrategy, useClass: PathLocationStrategy },
    provideHttpClient(withInterceptors([tokenInterceptor, apiResponseInterceptor])),
    MessageService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
