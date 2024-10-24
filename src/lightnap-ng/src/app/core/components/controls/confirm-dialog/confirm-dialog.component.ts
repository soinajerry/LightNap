import { Component, ElementRef, Input, TemplateRef } from "@angular/core";
import { ButtonSeverity } from "@core/models";
import { PrimeIcons } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { ConfirmDialogModule } from "primeng/confirmdialog";

@Component({
  standalone: true,
  selector: "confirm-dialog",
  templateUrl: "./confirm-dialog.component.html",
  imports: [ConfirmDialogModule, ButtonModule],
})
export class ConfirmDialogComponent {
    @Input() confirmText = "Confirm";
    @Input() confirmSeverity: ButtonSeverity = "danger";
    @Input() confirmIcon = PrimeIcons.TRASH;
    @Input() rejectText = "Cancel";
    @Input() rejectSeverity: ButtonSeverity = "secondary";
    @Input() rejectIcon = PrimeIcons.UNDO;
    @Input() key = "";
    @Input() appendTo?: HTMLElement | ElementRef | TemplateRef<any> | string | null | undefined | any;
}
