export interface RegisterRequest {
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
    rememberMe: boolean;
    deviceDetails: string;
}
