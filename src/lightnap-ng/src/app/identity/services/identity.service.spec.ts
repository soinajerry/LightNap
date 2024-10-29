import { TestBed } from '@angular/core/testing';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ErrorApiResponse, NewPasswordRequest, RegisterRequest, ResetPasswordRequest, SuccessApiResponse, TimerService, VerifyCodeRequest } from '@core';
import { of } from 'rxjs';
import { DataService } from './data.service';
import { IdentityService } from './identity.service';

describe('IdentityService', () => {
    let service: IdentityService;
    let dataServiceSpy: jasmine.SpyObj<DataService>;
    let timerServiceSpy: jasmine.SpyObj<TimerService>;
    // Using a valid JWT token for testing purposes. IdentityService will try to parse it so it might as well work.
    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
    
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

        dataServiceSpy = TestBed.inject(DataService) as jasmine.SpyObj<DataService>;
        dataServiceSpy.getAccessToken.and.returnValue(of(new ErrorApiResponse(["Unauthorized by default"])));

        service = TestBed.inject(IdentityService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should initialize and try to refresh token', () => {
        service = TestBed.inject(IdentityService);
        expect(timerServiceSpy.watchTimer$).toHaveBeenCalled();
        expect(dataServiceSpy.getAccessToken).toHaveBeenCalled();
    });

    it('should log in and set token', () => {
        const loginRequest = <any>{};
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
        dataServiceSpy.register.and.returnValue(of(new SuccessApiResponse({ bearerToken: token, twoFactorRequired: false })));
        service.register(registerRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.register).toHaveBeenCalledWith(registerRequest);
    });

    it('should verify code and set token', () => {
        const verifyCodeRequest: VerifyCodeRequest = <any>{};
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
        dataServiceSpy.newPassword.and.returnValue(of(new SuccessApiResponse(token)));
        service.newPassword(newPasswordRequest).subscribe(() => {
            expect(service.getBearerToken()).toBe(`Bearer ${token}`);
        });
        expect(dataServiceSpy.newPassword).toHaveBeenCalledWith(newPasswordRequest);
    });
});
