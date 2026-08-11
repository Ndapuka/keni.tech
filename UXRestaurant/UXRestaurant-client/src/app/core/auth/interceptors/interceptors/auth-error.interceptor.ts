// core/auth/interceptors/auth-error.interceptor.ts
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { AuthError } from '../../models/auth-error.model';

export const authErrorInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            const authError = mapToAuthError(error);
            return throwError(() => authError);
        })
    );
};

function mapToAuthError(error: HttpErrorResponse): AuthError {
    const serverMessage = extractServerMessage(error);

    switch (error.status) {
        case 401:
            return { code: 'INVALID_CREDENTIALS', message: serverMessage ?? 'Email ou password incorretos.', status: 401 };
        case 409:
            return { code: 'EMAIL_TAKEN', message: serverMessage ?? 'Este email já está registado.', status: 409 };
        case 400:
        case 422:
            return { code: 'VALIDATION', message: serverMessage ?? 'Dados inválidos.', status: error.status };
        case 0:
            return { code: 'SERVER', message: 'Não foi possível ligar ao servidor.', status: 0 };
        default:
            // cobre também o 500 do teu backend quando devolve mensagem de negócio no corpo
            // (ex: "Token inválido não encontrado")
            return { code: serverMessage ? 'VALIDATION' : 'UNKNOWN', message: serverMessage ?? 'Ocorreu um erro inesperado. Tenta novamente.', status: error.status };
    }
}

function extractServerMessage(error: HttpErrorResponse): string | null {
    return error.error?.message ?? error.error?.Message ?? error.error?.title ?? null;
}