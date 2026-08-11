// features/auth/pages/login/login/login.ts
import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    Validators,
    ReactiveFormsModule
} from '@angular/forms';

import { AuthFacadeService } from '../../../../core/auth/services/auth-facade.service';
import { AuthError } from '../../../../core/auth/models/auth-error.model';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './login.html',
    styleUrls: ['./login.scss']
})
export class LoginComponent {

    private fb = inject(FormBuilder);
    private authFacade = inject(AuthFacadeService);

    @Output()
    goToRegister = new EventEmitter<void>();

    @Output()
    goToForgotPassword = new EventEmitter<void>();

    @Output()
    loginSuccess = new EventEmitter<void>();

    loginForm: FormGroup = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required]
    });

    errorMessage = '';
    isSubmitting = false;

    openRegister(): void {
        this.goToRegister.emit();
    }

    openForgotPassword(): void {
        this.goToForgotPassword.emit();
    }

    login(): void {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }

        this.errorMessage = '';
        this.isSubmitting = true;

        this.authFacade.login(this.loginForm.value).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.loginSuccess.emit();
            },
            error: (err: AuthError) => {
                this.isSubmitting = false;
                this.errorMessage = err.message;
            }
        });
    }
}