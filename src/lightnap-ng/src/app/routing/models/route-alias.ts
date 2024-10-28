export type RouteAlias =
    // Public
    | 'landing'
    | 'about'
    | 'terms-and-conditions'
    | 'privacy-policy'
    | 'access-denied'
    | 'error'
    | 'not-found'

    // Secure
    | 'home'

    // Admin
    | 'admin-home'
    | 'admin-users'
    | 'admin-user'
    | 'admin-roles'
    | 'admin-role'

    // Profile
    | 'profile'
    | 'devices'

    // Identity
    | 'login'
    | 'reset-password'
    | 'reset-instructions-sent'
    | 'change-password'
    | 'verify-code'
    | 'register';
