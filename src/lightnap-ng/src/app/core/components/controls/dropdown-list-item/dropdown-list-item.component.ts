
import { Component, Input } from "@angular/core";
import { ListItem } from "@core";

@Component({
    selector: 'dropdown-list-item',
    templateUrl: './dropdown-list-item.component.html',
    imports: [],
    standalone: true,
})
export class DropdownListItemComponent {
    @Input() label = "";
    @Input() description?: string;

    @Input() set listItem(value: ListItem<any>) {
        this.label = value.label;
        this.description = value.description;
    }
}
