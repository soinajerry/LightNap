import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';

@Component({
    standalone: true,
    templateUrl: './users.component.html',
    imports: [CardModule]
})
export class UsersComponent {
}
