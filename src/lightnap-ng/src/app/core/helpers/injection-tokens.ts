import { InjectionToken } from "@angular/core";

/**
 * Injection token for the root URL of the API.
 *
 * This token can be used to inject the root URL of the API into Angular services or components.
 *
 * @example
 * ```typescript
 * constructor(@Inject(API_URL_ROOT) private apiUrlRoot: string) {}
 * ```
 */
export const API_URL_ROOT = new InjectionToken<string>('API_URL_ROOT');
