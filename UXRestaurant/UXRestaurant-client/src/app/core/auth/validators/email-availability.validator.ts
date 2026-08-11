import { inject } from '@angular/core';
import { AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { catchError, map, of, switchMap, timer } from 'rxjs';
import { AuthFacadeService } from '../services/auth-facade.service';

export function emailAvailabilityValidator(): AsyncValidatorFn {
    const authFacade = inject(AuthFacadeService);

    return (control: AbstractControl): ReturnType<AsyncValidatorFn> => {
        if (!control.value || control.hasError('email') || control.hasError('required')) {
            return of(null);
        }

        return timer(500).pipe(
            switchMap(() => authFacade.checkEmailAvailability(control.value)),
            map((res) => (res.isAvailable ? null : { emailTaken: true })),
            catchError(() => of(null))
        );
    };
}