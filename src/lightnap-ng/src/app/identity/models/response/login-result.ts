export interface LoginResult {
    twoFactorRequired: boolean;
    bearerToken?: string;
}
