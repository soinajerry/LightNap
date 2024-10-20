import { CommonModule } from "@angular/common";
import { Component, Input, OnChanges, SimpleChanges } from "@angular/core";
import { ApiResponse } from "@core";

@Component({
  selector: "error-list",
  standalone: true,
  templateUrl: "./error-list.component.html",
  imports: [CommonModule],
})
export class ErrorListComponent implements OnChanges {
  @Input() error?: string;
  @Input() errors?: Array<string>;
  @Input() apiResponse?: ApiResponse<any>;

  errorList = new Array<string>();

  ngOnChanges(changes: SimpleChanges): void {
    if (this.error) {
      this.errorList = [this.error];
    } else if (this.errors?.length) {
      this.errorList = [...this.errors];
    } else if (this.apiResponse?.errorMessages?.length) {
      this.errorList = [...this.apiResponse.errorMessages];
    } else {
      this.errorList = [];
    }
  }
}
