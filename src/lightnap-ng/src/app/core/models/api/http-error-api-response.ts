import { HttpErrorResponse } from "@angular/common/http";
import { ApiResponse } from "./api-response";
import { ApiResponseType } from "./api-response-type";
import { environment } from "src/environments/environment";

/**
 * Represents an HTTP error response from an API.
 *
 * @template T - The type of the result expected from the API response.
 */
export class HttpErrorApiResponse<T> implements ApiResponse<T> {
  /**
   * The result of the API response, if any.
   */
  result?: T;

  /**
   * The type of the API response.
   * Defaults to "UnexpectedError".
   */
  type: ApiResponseType = "UnexpectedError";

  /**
   * A list of error messages associated with the API response.
   */
  errorMessages: Array<string>;

  /**
   * Constructs an instance of `HttpErrorApiResponse`.
   *
   * @param error - The HTTP error response received from the API.
   */
  constructor(error: HttpErrorResponse) {
    switch (error.status) {
      case 0:
        this.errorMessages = ["We were unable to connect to the service."];
        break;

      default:
        this.errorMessages = ["An unexpected error occurred"];
        break;
    }

    if (!environment.production) {
      this.errorMessages.push(`DEBUG: ${error.message}`);
    }
  }
}
