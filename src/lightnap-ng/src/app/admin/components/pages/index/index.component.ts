import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';

@Component({
    standalone: true,
    templateUrl: './index.component.html',
    imports: [CardModule]
})
export class IndexComponent {
}
