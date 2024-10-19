import { InjectionToken } from "@angular/core";
import { RouteHelper } from "./route-helper";

export const API_URL_ROOT = new InjectionToken<string>('API_URL_ROOT');
export const ROUTE_HELPER = new InjectionToken<RouteHelper>('ROUTE_HELPER');
