import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { authTokenInterceptor } from './core/auth/interceptors/interceptors/auth-token.interceptor';
import { authErrorInterceptor } from './core/auth/interceptors/interceptors/auth-error.interceptor';


export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(
      withInterceptors([authTokenInterceptor, authErrorInterceptor]) //error interceptor is not defined in the provided code, you may need to import it if it exists  
    ),
    provideRouter(routes)
  ]
};
