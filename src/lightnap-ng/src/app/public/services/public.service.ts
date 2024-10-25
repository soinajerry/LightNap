import { inject, Injectable } from "@angular/core";
import { BrowserSettings, ChangePasswordRequest, StyleSettings, UpdateProfileRequest } from "@profile";
import { DataService } from "./data.service";
import { delay, map, of, switchMap, tap } from "rxjs";
import { ApiResponse, ErrorApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class PublicService {
  #dataService = inject(DataService);

}
