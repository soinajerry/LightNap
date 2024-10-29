/**
 * Represents the result of a login attempt.
 */
export interface LoginResult {
    /**
     * Indicates whether two-factor authentication is required. Otherwise there will be a bearerToken.
     */
    twoFactorRequired: boolean;

    /**
     * The bearer token received upon successful login, if two-factor authentication is not required.
     */
    bearerToken?: string;
}
