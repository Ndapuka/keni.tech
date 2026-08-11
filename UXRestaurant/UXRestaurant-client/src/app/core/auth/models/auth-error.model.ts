// core/auth/models/auth-error.model.ts
export interface AuthError {
    code: 'INVALID_CREDENTIALS' | 'EMAIL_TAKEN' | 'VALIDATION' | 'SERVER' | 'UNKNOWN';
    message: string;
    status: number;
}