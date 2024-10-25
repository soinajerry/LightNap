import { Component } from '@angular/core';
import { CardModule } from 'primeng/card';

@Component({
    selector: 'app-home-index',
    standalone: true,
    templateUrl: './index.component.html',
    imports: [CardModule]
})
export class IndexComponent {
}
