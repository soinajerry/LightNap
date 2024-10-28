import { discardPeriodicTasks, TestBed } from '@angular/core/testing';
import { TimerService } from './timer.service';
import { fakeAsync, tick } from '@angular/core/testing';

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
        expect(() => service.watchTimer$(-1000)).toThrowError("Intervals must be positive: '-1000' not valid");
    });

    it('should return an EventEmitter for a valid interval', () => {
        const emitter = service.watchTimer$(1000);
        expect(emitter).toBeTruthy();
    });

    it('should emit events at the specified interval', fakeAsync(() => {
        const milliseconds = 1000;
        const emitter = service.watchTimer$(milliseconds);
        let emitted = false;
        emitter.subscribe(() => emitted = true);

        tick(milliseconds);
        expect(emitted).toBeTrue();
        discardPeriodicTasks();
    }));

    it('should reuse the same EventEmitter for the same interval', () => {
        const emitter1 = service.watchTimer$(1000);
        const emitter2 = service.watchTimer$(1000);
        expect(emitter1).toBe(emitter2);
    });

    it('should create different EventEmitters for different intervals', () => {
        const emitter1 = service.watchTimer$(1000);
        const emitter2 = service.watchTimer$(2000);
        expect(emitter1).not.toBe(emitter2);
    });
});
