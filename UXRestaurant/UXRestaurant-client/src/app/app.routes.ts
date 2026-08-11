import { Routes } from '@angular/router';

export const routes: Routes = [{
    path: '',
    loadComponent: () =>
        import('./features/landing/pages/home/home').then(m => m.Home)
},
{
    path: 'confirm-email',
    loadComponent: () =>
        import('./features/auth/pages/confirm-email/confirm-email').then(m => m.ConfirmEmail)
},
{
    path: 'reset-password',
    loadComponent: () =>
        import('./features/auth/pages/reset-password/reset-password').then(m => m.ResetPassword)
},
{
    path: '**',
    redirectTo: ''
}
];
