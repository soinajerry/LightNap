export interface NewPasswordRequest {
    email: string;
    password: string;
    token: string;
    rememberMe: boolean;
    deviceDetails: string;
}
