import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthSessionService } from '../../services/auth-session.service';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
    const session = inject(AuthSessionService);
    const token = session.token();

    if (!token) {
        return next(req);
    }

    const cloned = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
    });

    return next(cloned);
};