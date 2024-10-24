import { Component, inject, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
    #primengConfig = inject(PrimeNGConfig);

    ngOnInit() {
        this.#primengConfig.ripple = true;
    }
}
