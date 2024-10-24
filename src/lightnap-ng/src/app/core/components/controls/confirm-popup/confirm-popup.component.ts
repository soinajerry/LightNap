import { Component, Input } from "@angular/core";
import { ButtonSeverity } from "@core/models";
import { PrimeIcons } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { ConfirmPopupModule } from "primeng/confirmpopup";

@Component({
  standalone: true,
  selector: "confirm-popup",
  templateUrl: "./confirm-popup.component.html",
  imports: [ConfirmPopupModule, ButtonModule],
})
export class ConfirmPopupComponent {
    @Input() confirmText = "Confirm";
    @Input() confirmSeverity: ButtonSeverity = "danger";
    @Input() confirmIcon = PrimeIcons.TRASH;
    @Input() rejectText = "Cancel";
    @Input() rejectSeverity: ButtonSeverity = "secondary";
    @Input() rejectIcon = PrimeIcons.UNDO;
    @Input() key = "";
}
