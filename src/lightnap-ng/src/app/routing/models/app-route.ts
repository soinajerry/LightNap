import { Route } from "@angular/router";
import { RouteData } from "./route-data";

export interface AppRoute extends Route {
    children?: Array<AppRoute>;
    data?: RouteData;
}
