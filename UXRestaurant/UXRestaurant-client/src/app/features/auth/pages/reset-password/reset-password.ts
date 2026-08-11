// features/auth/pages/reset-password/reset-password.ts
import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthFacadeService } from '../../../../core/auth/services/auth-facade.service';
import { AuthError } from '../../../../core/auth/models/auth-error.model';
import { AuthMessage } from '../../components/auth-message/auth-message';

@Component({
    selector: 'app-reset-password',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, AuthMessage],
    templateUrl: './reset-password.html',
    styleUrls: ['./reset-password.scss']
})
export class ResetPassword implements OnInit, OnDestroy {

    private fb = inject(FormBuilder);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private authFacade = inject(AuthFacadeService);

    private token = '';
    private redirectTimeout?: ReturnType<typeof setTimeout>;

    tokenMissing = false;

    resetForm: FormGroup = this.fb.group({
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
    });

    isSubmitting = false;
    showMessage = false;
    isError = false;
    messageTitle = '';
    messageText = '';

    ngOnInit(): void {
        const token = this.route.snapshot.queryParamMap.get('token');

        if (!token) {
            this.tokenMissing = true;
            return;
        }

        this.token = token;
    }

    ngOnDestroy(): void {
        clearTimeout(this.redirectTimeout);
    }

    submit(): void {
        if (this.resetForm.invalid) {
            this.resetForm.markAllAsTouched();
            return;
        }

        const { newPassword, confirmPassword } = this.resetForm.value;

        if (newPassword !== confirmPassword) {
            this.resetForm.get('confirmPassword')?.setErrors({ mismatch: true });
            return;
        }

        this.isSubmitting = true;

        this.authFacade.resetPassword(this.token, newPassword).subscribe({
            next: () => {
                this.isSubmitting = false;
                this.isError = false;
                this.messageTitle = 'Password redefinida';
                this.messageText = 'A tua password foi alterada com sucesso. A redirecionar para o login...';
                this.showMessage = true;
                this.redirectTimeout = setTimeout(() => this.goToLogin(), 2500);
            },
            error: (err: AuthError) => {
                this.isSubmitting = false;
                this.isError = true;
                this.messageTitle = 'Não foi possível redefinir';
                this.messageText = err.message;
                this.showMessage = true;
                // erro não redireciona sozinho — o utilizador precisa de ler o motivo
            }
        });
    }

    goToLogin(): void {
        this.router.navigate(['/'], { queryParams: { openAuth: 'login' } });
    }
}