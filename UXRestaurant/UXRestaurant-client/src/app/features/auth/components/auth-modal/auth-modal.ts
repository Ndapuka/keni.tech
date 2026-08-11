// features/auth/components/auth-modal/auth-modal.ts
import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginComponent } from '../../pages/login/login';
import { Register } from '../../pages/register/register';
import { ForgotPassword } from '../../pages/forgot-password/forgot-password';

@Component({
    selector: 'app-auth-modal',
    standalone: true,
    imports: [
        CommonModule,
        LoginComponent,
        Register,
        ForgotPassword
    ],
    templateUrl: './auth-modal.html',
    styleUrls: ['./auth-modal.scss']
})
export class AuthModal {

    @Output()
    closed = new EventEmitter<void>();

    @Output()
    loginSuccess = new EventEmitter<void>();

    currentView: 'login' | 'register' | 'forgot-password' = 'login';

    closeModal(): void {
        this.closed.emit();
    }

    openLogin(): void {
        this.currentView = 'login';
    }

    openRegister(): void {
        this.currentView = 'register';
    }

    openForgotPassword(): void {
        this.currentView = 'forgot-password';
    }

    onLoginSuccess(): void {
        this.loginSuccess.emit();
    }
}