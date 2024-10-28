import { Pipe, PipeTransform, inject } from "@angular/core";
import { RouteAliasService } from "@routing/services/route-alias-service";
import { RouteAlias } from "@routing/models/route-alias";

@Pipe({
  name: "route",
  standalone: true,
})
export class RoutePipe implements PipeTransform {
  #routeAliasService = inject(RouteAliasService);

  transform(view: RouteAlias, value?: any) {
    return this.#routeAliasService.getRoute(view, value);
  }
}
