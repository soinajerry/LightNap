import { TestBed } from '@angular/core/testing';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NewPasswordRequest, RegisterRequest, ResetPasswordRequest, SuccessApiResponse, TimerService, VerifyCodeRequest } from '@core';
import { of } from 'rxjs';
import { DataService } from './data.service';
import { IdentityService } from './identity.service';

describe('IdentityService', () => {
    let service: IdentityService;
    let dataServiceSpy: jasmine.SpyObj<DataService>;
    let timerServiceSpy: jasmine.SpyObj<TimerService>;

    beforeEach(() => {
        const dataSpy = jasmine.createSpyObj('DataService', ['getAccessToken', 'logIn', 'register', 'logOut', 'verifyCode', 'resetPassword', 'newPassword']);
        const timerSpy = jasmine.createSpyObj('TimerService', ['watchTimer$']);

        TestBed.configureTestingModule({
            providers: [
                IdentityService,
                { provide: DataService, useValue: dataSpy },
                { provide: TimerService, useValue: timerSpy },
                JwtHelperService
            ]
        });

        timerServiceSpy = TestBed.inject(TimerService) as jasmine.SpyObj<TimerService>;
        timerServiceSpy.watchTimer$.and.returnValue(of(0));

        service = TestBed.inject(IdentityService);
        dataServiceSpy = TestBed.inject(DataService) as jasmine.SpyObj<DataService>;
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should initialize and try to refresh token', () => {
        timerServiceSpy.watchTimer$.and.returnValue(of(0));
        service = TestBed.inject(IdentityService);
        expect(timerServiceSpy.watchTimer$).toHaveBeenCalled();
        expect(dataServiceSpy.getAccessToken).toHaveBeenCalled();
    });

    it('should log in and set token', () => {
        const loginRequest = <any>{};
        const token = 'fake-jwt-token';
        dataServiceSpy.logIn.and.returnValue(of(new SuccessApiResponse({ bearerToken: token, twoFactorRequired: false })));
        service.logIn(loginRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.logIn).toHaveBeenCalledWith(loginRequest);
    });

    it('should log out and clear token', () => {
        dataServiceSpy.logOut.and.returnValue(of(new SuccessApiResponse(true)));
        service.logOut().subscribe(() => {
            expect(service.getBearerToken()).toBeUndefined();
        });
        expect(dataServiceSpy.logOut).toHaveBeenCalled();
    });

    it('should register and set token', () => {
        const registerRequest: RegisterRequest = <any>{};
        const token = 'fake-jwt-token';
        dataServiceSpy.register.and.returnValue(of(new SuccessApiResponse({ bearerToken: token, twoFactorRequired: false })));
        service.register(registerRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.register).toHaveBeenCalledWith(registerRequest);
    });

    it('should verify code and set token', () => {
        const verifyCodeRequest: VerifyCodeRequest = <any>{};
        const token = 'fake-jwt-token';
        dataServiceSpy.verifyCode.and.returnValue(of(new SuccessApiResponse(token)));
        service.verifyCode(verifyCodeRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.verifyCode).toHaveBeenCalledWith(verifyCodeRequest);
    });

    it('should reset password', () => {
        const resetPasswordRequest: ResetPasswordRequest = <any>{ };
        dataServiceSpy.resetPassword.and.returnValue(of(<any>{}));
        service.resetPassword(resetPasswordRequest).subscribe();
        expect(dataServiceSpy.resetPassword).toHaveBeenCalledWith(resetPasswordRequest);
    });

    it('should set new password and set token', () => {
        const newPasswordRequest: NewPasswordRequest = <any>{};
        const token = 'fake-jwt-token';
        dataServiceSpy.newPassword.and.returnValue(of(new SuccessApiResponse(token)));
        service.newPassword(newPasswordRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.newPassword).toHaveBeenCalledWith(newPasswordRequest);
    });
});
