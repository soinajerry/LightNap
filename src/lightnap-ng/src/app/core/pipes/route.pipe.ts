import { Pipe, PipeTransform, inject } from "@angular/core";
import { ROUTE_HELPER, SupportedRoutes } from "@core";

@Pipe({
  name: "route",
  standalone: true,
})
export class RoutePipe implements PipeTransform {
  #routeHelper = inject(ROUTE_HELPER);

  transform(view: SupportedRoutes, value?: any) {
    return this.#routeHelper.getRoute(view, value);
  }
}
