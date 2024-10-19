import { EventEmitter, Injectable } from "@angular/core";
import { interval } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class TimerService {
  #timers: { [index: number]: EventEmitter<void> } = {};

  watchTimer$(milliseconds: number) {
    if (milliseconds < 0) {
      throw new Error(`Intervals must be positive: '${milliseconds}' not valid`);
    }

    if (!this.#timers[milliseconds]) {
      this.#timers[milliseconds] = new EventEmitter();
      interval(milliseconds).subscribe({
        next: () => this.#timers[milliseconds].emit(),
      });
    }
    return this.#timers[milliseconds];
  }
}
