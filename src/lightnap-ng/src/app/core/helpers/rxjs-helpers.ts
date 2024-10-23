import { distinctUntilChanged, Observable } from "rxjs";

export function distinctUntilJsonChanged<T>() {
    return (source$: Observable<T>) =>
      source$.pipe(distinctUntilChanged((original, incoming) => JSON.stringify(original) === JSON.stringify(incoming)));
  }
