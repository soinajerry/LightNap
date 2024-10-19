export type SupportedRoutes =
    // Public
    | 'landing'
    | 'about'
    | 'terms-and-conditions'
    | 'privacy-policy'
    | 'access-denied'

    // Secure
    | 'home'

    // Admin
    | 'admin'
    | 'admin-users'
    | 'admin-user'

    // Profile
    | 'profile'
    | 'devices'

    // Identity
    | 'login'
    | 'reset-password'
    | 'reset-instructions-sent'
    | 'new-password'
    | 'change-password'
    | 'verify-code'
    | 'register';
