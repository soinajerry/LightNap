import { CommonModule } from "@angular/common";
import { Component, ContentChild, Input, TemplateRef } from "@angular/core";
import { ApiResponse } from "@core";
import { Observable } from "rxjs";
import { ErrorListComponent } from "../error-list/error-list.component";

@Component({
  selector: "api-response",
  standalone: true,
  templateUrl: "./api-response.component.html",
  imports: [CommonModule, ErrorListComponent],
})
export class ApiResponseComponent {
  @Input({ required: true }) apiResponse: Observable<ApiResponse<any>>;
  @Input() errorMessage = "An error occurred";
  @Input() loadingMessage = "Loading...";

  @ContentChild('success') successTemplateRef: TemplateRef<any>;
  @ContentChild('error') errorTemplateRef: TemplateRef<any>;
  @ContentChild('loading') loadingTemplateRef: TemplateRef<any>;

}
