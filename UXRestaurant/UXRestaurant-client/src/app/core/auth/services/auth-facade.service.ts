import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { AuthApiService } from './auth-api.service';
import { AuthSessionService } from './auth-session.service';
import { LoginRequestDto } from '../models/login-request.dto';
import { RegisterRequestDto } from '../models/register-request.dto';
import { RegisterResponseDto } from '../models/register-response.dto';
import { LoginResponseDto } from '../models/login-response.dto';
import { toAuthUser } from '../models/auth-user.model';


@Injectable({ providedIn: 'root' })
export class AuthFacadeService {

    private api = inject(AuthApiService);
    private session = inject(AuthSessionService);

    readonly user = this.session.user;

    readonly isAuthenticated = this.session.isAuthenticated;

    login(request: LoginRequestDto): Observable<LoginResponseDto> {
        return this.api.login(request).pipe(
            tap((response) => {
                this.session.setSession({
                    token: response.token,
                    refreshToken: response.refreshToken,
                    user: toAuthUser(response)

                });
            })
        );
    }

    register(request: RegisterRequestDto): Observable<RegisterResponseDto> {
        // registo não abre sessão automaticamente — utilizador confirma email e faz login manual
        return this.api.register(request);
    }
    checkEmailAvailability(email: string): Observable<{ isAvailable: boolean }> {
        return this.api.checkEmailAvailability(email);
    }

    forgotPassword(email: string): Observable<{ message: string }> {
        return this.api.forgotPassword(email);
    }

    resetPassword(token: string, newPassword: string): Observable<{ message: string }> {
        return this.api.resetPassword(token, newPassword);
    }

    confirmEmail(token: string): Observable<void> {
        return this.api.confirmEmail(token);
    }

    logout(): void {
        this.session.clearSession();
    }
}