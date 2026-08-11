import { Injectable, computed, signal } from '@angular/core';
import { AuthUser } from '../models/auth-user.model';
import { AuthSession } from '../models/auth-session.model';

const STORAGE_KEY = 'keni.auth.session';

@Injectable({ providedIn: 'root' })
export class AuthSessionService {

    private readonly sessionSignal = signal<AuthSession | null>(this.readFromStorage());

    readonly user = computed<AuthUser | null>(() => this.sessionSignal()?.user ?? null);
    readonly isAuthenticated = computed(() => this.sessionSignal() !== null);
    readonly token = computed(() => this.sessionSignal()?.token ?? null);

    setSession(session: AuthSession): void {
        this.sessionSignal.set(session);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
    }

    clearSession(): void {
        this.sessionSignal.set(null);
        localStorage.removeItem(STORAGE_KEY);
    }

    getRefreshToken(): string | null {
        return this.sessionSignal()?.refreshToken ?? null;
    }

    private readFromStorage(): AuthSession | null {
        const raw = localStorage.getItem(STORAGE_KEY);
        if (!raw) return null;

        try {
            return JSON.parse(raw) as AuthSession;
        } catch {
            localStorage.removeItem(STORAGE_KEY);
            return null;
        }
    }
}