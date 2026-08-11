import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

import { LoginRequestDto } from '../models/login-request.dto';
import { LoginResponseDto } from '../models/login-response.dto';
import { RegisterRequestDto } from '../models/register-request.dto';
import { RegisterResponseDto } from '../models/register-response.dto';


@Injectable({ providedIn: 'root' })
export class AuthApiService {

    private readonly baseUrl = environment.userServiceUrl;

    constructor(private http: HttpClient) { }

    login(request: LoginRequestDto): Observable<LoginResponseDto> {
        return this.http.post<LoginResponseDto>(`${this.baseUrl}/login`, request);
    }


    register(request: RegisterRequestDto): Observable<RegisterResponseDto> {
        return this.http.post<RegisterResponseDto>(`${this.baseUrl}/register`, request);
    }
    checkEmailAvailability(email: string): Observable<{ isAvailable: boolean }> {
        return this.http.get<{ isAvailable: boolean }>(`${this.baseUrl}/check-email`, {
            params: { email }
        });
    }

    forgotPassword(email: string): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(`${this.baseUrl}/forgot-password`, { email });
    }

    resetPassword(token: string, newPassword: string): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(`${this.baseUrl}/reset-password`, { token, newPassword });
    }

    confirmEmail(token: string): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/confirm-email`, { token });
    }

    refreshToken(refreshToken: string): Observable<LoginResponseDto> {
        return this.http.post<LoginResponseDto>(`${this.baseUrl}/refresh`, { refreshToken });
    }
}