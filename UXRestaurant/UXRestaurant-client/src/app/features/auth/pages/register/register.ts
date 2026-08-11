// features/auth/pages/register/register/register.ts
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
import { AuthMessage } from '../../components/auth-message/auth-message';
import { emailAvailabilityValidator } from '../../../../core/auth/validators/email-availability.validator';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, AuthMessage],
    templateUrl: './register.html',
    styleUrls: ['./register.scss']
})
export class Register {

    private fb = inject(FormBuilder);
    private authFacade = inject(AuthFacadeService);

    @Output()
    goToLogin = new EventEmitter<void>();

    registerForm: FormGroup = this.fb.group({
        personName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email], [emailAvailabilityValidator()]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        gender: ['', Validators.required],
        phoneNumber: ['']
    });

    showMessage = false;
    isError = false;
    messageTitle = '';
    messageText = '';
    isSubmitting = false;

    register(): void {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        this.isSubmitting = true;

        this.authFacade.register(this.registerForm.value).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.isError = false;
                this.messageTitle = 'Conta criada';
                this.messageText = 'Enviámos um email de confirmação. Já podes iniciar sessão.';
                this.showMessage = true;
            },
            error: (err: AuthError) => {
                this.isSubmitting = false;
                this.isError = true;
                this.messageTitle = 'Não foi possível criar a conta';
                this.messageText = err.message;
                this.showMessage = true;
            }
        });
    }

    onMessageButtonClick(): void {
        this.showMessage = false;

        if (!this.isError) {
            this.openLogin();
        }
    }

    openLogin(): void {
        this.goToLogin.emit();
    }
}