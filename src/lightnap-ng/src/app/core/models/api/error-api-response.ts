import { ApiResponse } from "./api-response";
import { ApiResponseType } from "./api-response-type";

export class ErrorApiResponse<T> implements ApiResponse<T> {
  type: ApiResponseType = "Error";
  succeeded = false;
  requiresReauthorization = false;
  result = undefined;

  constructor(public errorMessages: Array<string>) {}
}
