import { SupportedRoutes } from "./supported-routes";

export interface RouteHelper {
    getRoute(view: SupportedRoutes, value?: any): Array<string>;
    navigate(view: SupportedRoutes, value?: any): Promise<boolean>;
    navigateWithReplace(view: SupportedRoutes, value?: any): Promise<boolean>;
}
