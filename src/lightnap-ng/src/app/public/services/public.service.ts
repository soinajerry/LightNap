import { inject, Injectable } from "@angular/core";
import { DataService } from "./data.service";

@Injectable({
  providedIn: "root",
})
export class PublicService {
  #dataService = inject(DataService);
}
