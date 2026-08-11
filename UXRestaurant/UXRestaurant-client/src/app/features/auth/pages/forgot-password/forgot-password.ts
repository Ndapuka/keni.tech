// features/auth/components/forgot-password/forgot-password.ts
import { Component, EventEmitter, Output, inject, signal } from '@angular/core';

import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';

import { AuthFacadeService } from '../../../../core/auth/services/auth-facade.service';
import { AuthError } from '../../../../core/auth/models/auth-error.model';
import { AuthMessage } from '../../components/auth-message/auth-message';

@Component({
    selector: 'app-forgot-password',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, AuthMessage],
    templateUrl: './forgot-password.html',
    styleUrls: ['./forgot-password.scss']
})
export class ForgotPassword {

    private fb = inject(FormBuilder);
    private authFacade = inject(AuthFacadeService);

    @Output()
    goToLogin = new EventEmitter<void>();

    forgotPasswordForm: FormGroup = this.fb.group({
        email: ['', [Validators.required, Validators.email]]
    });

    showMessage = signal(false);
    isError = signal(false);
    messageTitle = signal('');
    messageText = signal('');
    isSubmitting = signal(false);

    sendRecoveryEmail(): void {
        if (this.forgotPasswordForm.invalid) {
            this.forgotPasswordForm.markAllAsTouched();
            return;
        }

        this.isSubmitting.set(true);
        const email = this.forgotPasswordForm.value.email;

        this.authFacade.forgotPassword(email).subscribe({
            next: () => {
                this.isSubmitting.set(false);
                this.isError.set(false);
                this.messageTitle.set('Verifica o teu email');
                this.messageText.set('Se existir uma conta associada a este email, enviámos um link para redefinires a password. Consulta a tua caixa de entrada.');
                this.showMessage.set(true);
            },
            error: (err: AuthError) => {
                this.isSubmitting = signal(false);
                this.isError.set(true);
                this.messageTitle.set('Não foi possível enviar');
                this.messageText.set(err.message);
                this.showMessage.set(true);
            }
        });
    }

    backToLogin(): void {
        this.goToLogin.emit();
    }
}