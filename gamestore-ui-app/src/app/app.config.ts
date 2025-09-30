import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection, inject } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { LocalizationService } from './services/localization.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          const localizationService = inject(LocalizationService);
          const currentLanguage = localizationService.getCurrentLanguage();
          
          const localizedRequest = req.clone({
            setHeaders: {
              'Accept-Language': currentLanguage.code
            }
          });

          console.log(`🌐 Adding Accept-Language header: ${currentLanguage.code} to request: ${req.url}`);
          return next(localizedRequest);
        }
      ])
    )
  ]
};
