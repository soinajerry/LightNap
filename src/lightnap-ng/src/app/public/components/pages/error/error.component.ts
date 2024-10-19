import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RoutePipe } from '@core';

@Component({
    selector: 'app-error',
    standalone: true,
    templateUrl: './error.component.html',
    imports: [RouterLink, RoutePipe]
})
export class ErrorComponent { }
