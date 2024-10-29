import { discardPeriodicTasks, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { TimerService } from './timer.service';
import { take } from 'rxjs/operators';

describe('TimerService', () => {
    let service: TimerService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(TimerService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should throw an error if interval is negative', () => {
        expect(() => service.watchTimer$(-1000)).toThrowError('Intervals must be positive: \'-1000\' not valid');
    });

    it('should return an observable that emits at the specified interval', fakeAsync(() => {
        const milliseconds = 1000;
        let emittedValue: number | undefined;
        const subscription = service.watchTimer$(milliseconds).pipe(take(1)).subscribe(value => {
            emittedValue = value;
        });

        tick(milliseconds);
        expect(emittedValue).toBe(milliseconds);
        discardPeriodicTasks();
    }));

    it('should return the same observable for the same interval', () => {
        const milliseconds = 1000;
        const observable1 = service.watchTimer$(milliseconds);
        const observable2 = service.watchTimer$(milliseconds);
        expect(observable1).toBe(observable2);
    });

    it('should create a new observable for a different interval', () => {
        const milliseconds1 = 1000;
        const milliseconds2 = 2000;
        const observable1 = service.watchTimer$(milliseconds1);
        const observable2 = service.watchTimer$(milliseconds2);
        expect(observable1).not.toBe(observable2);
    });
});
