// core/auth/models/auth-session.model.ts

import { AuthUser } from "./auth-user.model";

// O que fica persistido/em memória
export interface AuthSession {
    token: string;
    refreshToken: string;
    user: AuthUser;

}