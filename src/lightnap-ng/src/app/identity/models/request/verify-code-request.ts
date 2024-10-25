export interface VerifyCodeRequest {
    email: string;
    code: string;
    rememberMe: boolean;
    deviceDetails: string;
}
