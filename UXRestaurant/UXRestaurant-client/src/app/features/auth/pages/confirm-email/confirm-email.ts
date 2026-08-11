// features/auth/pages/confirm-email/confirm-email.ts
import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthFacadeService } from '../../../../core/auth/services/auth-facade.service';
import { AuthError } from '../../../../core/auth/models/auth-error.model';
import { AuthMessage } from '../../components/auth-message/auth-message';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [CommonModule, AuthMessage],
  templateUrl: './confirm-email.html',
  styleUrls: ['./confirm-email.scss']
})
export class ConfirmEmail implements OnInit, OnDestroy {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authFacade = inject(AuthFacadeService);

  private redirectTimeout?: ReturnType<typeof setTimeout>;

  loading = true;
  isError = false;
  title = '';
  message = '';

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.loading = false;
      this.isError = true;
      this.title = 'Link inválido';
      this.message = 'Não encontrámos um token de confirmação neste link.';
      return;
    }

    this.authFacade.confirmEmail(token).subscribe({
      next: () => {
        this.loading = false;
        this.isError = false;
        this.title = 'Email confirmado';
        this.message = 'A tua conta foi confirmada com sucesso. A redirecionar para o login...';
        this.scheduleRedirect();
      },
      error: (err: AuthError) => {
        this.loading = false;
        this.isError = true;
        this.title = 'Não foi possível confirmar';
        this.message = err.message;

      }
    });
  }

  ngOnDestroy(): void {
    clearTimeout(this.redirectTimeout);
  }

  private scheduleRedirect(): void {
    this.redirectTimeout = setTimeout(() => this.goToLogin(), 2500);
  }

  goToLogin(): void {
    this.router.navigate(['/'], { queryParams: { openAuth: 'login' } });
  }
}