// core/auth/models/auth-user.model.ts

import { LoginResponseDto } from "./login-response.dto";

// ViewModel: o que a app usa internamente, desacoplado do shape exato da API
export interface AuthUser {
    id: string;
    email: string;
    name: string;
    role: string;
}

export function toAuthUser(dto: LoginResponseDto): AuthUser {
    return {
        id: dto.userId,
        email: dto.email,
        name: dto.personName,
        role: dto.role
    };
}