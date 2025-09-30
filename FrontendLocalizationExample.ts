// ====================================================================
// ANGULAR FRONTEND INTEGRATION EXAMPLE
// ====================================================================

// 1. Update your Angular HTTP interceptor to send Accept-Language header
// File: gamestore-ui-app/src/app/interceptors/localization.interceptor.ts

import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { LocalizationService } from '../services/localization.service';

@Injectable()
export class LocalizationInterceptor implements HttpInterceptor {
  constructor(private localizationService: LocalizationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // Get current language from localization service
    const currentLanguage = this.localizationService.getCurrentLanguage();
    
    // Add Accept-Language header to all requests
    const localizedRequest = req.clone({
      setHeaders: {
        'Accept-Language': currentLanguage
      }
    });

    return next.handle(localizedRequest);
  }
}

// ====================================================================
// 2. Update your localization service to manage current language
// File: gamestore-ui-app/src/app/services/localization.service.ts (add to existing)

export class LocalizationService {
  private currentLanguage = 'en'; // Default to English

  getCurrentLanguage(): string {
    // Get from localStorage or component state
    return localStorage.getItem('selectedLanguage') || this.currentLanguage;
  }

  setCurrentLanguage(language: string): void {
    this.currentLanguage = language;
    localStorage.setItem('selectedLanguage', language);
    
    // Optionally refresh current page data
    // this.refreshCurrentData();
  }

  // Add this to your existing availableLanguages
  readonly availableLanguages: Language[] = [
    { code: 'en', name: 'English'},
    { code: 'ka', name: 'ქართული'},
    { code: 'de', name: 'Deutsch' }
  ];
}

// ====================================================================
// 3. Register the interceptor in your app.module.ts

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LocalizationInterceptor } from './interceptors/localization.interceptor';

@NgModule({
  // ... existing configuration
  providers: [
    // ... existing providers
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LocalizationInterceptor,
      multi: true
    }
  ]
})
export class AppModule { }

// ====================================================================
// 4. Update your game service to work with localized responses
// File: gamestore-ui-app/src/app/services/game.service.ts (example)

@Injectable()
export class GameService {
  constructor(
    private http: HttpClient,
    private localizationService: LocalizationService
  ) {}

  getGames(filters?: any): Observable<any> {
    // The Accept-Language header will be automatically added by interceptor
    return this.http.get<any>('/api/games', { params: filters });
  }

  getGameByKey(key: string): Observable<any> {
    // The Accept-Language header will be automatically added by interceptor
    return this.http.get<any>(\`/api/games/\${key}\`);
  }
}

// ====================================================================
// 5. Language switcher component example
// File: gamestore-ui-app/src/app/components/language-switcher.component.ts

@Component({
  selector: 'app-language-switcher',
  template: \`
    <select (change)="onLanguageChange($event)" [value]="currentLanguage">
      <option *ngFor="let lang of availableLanguages" [value]="lang.code">
        {{ lang.name }}
      </option>
    </select>
  \`
})
export class LanguageSwitcherComponent {
  availableLanguages = this.localizationService.availableLanguages;
  currentLanguage = this.localizationService.getCurrentLanguage();

  constructor(private localizationService: LocalizationService) {}

  onLanguageChange(event: any): void {
    const newLanguage = event.target.value;
    this.localizationService.setCurrentLanguage(newLanguage);
    
    // Optionally reload the page or refresh data
    window.location.reload(); // Simple approach
    // OR trigger data refresh without page reload
  }
}

// ====================================================================
// TESTING THE IMPLEMENTATION
// ====================================================================

// 1. Start your backend with the localization service
// 2. Use browser dev tools Network tab
// 3. Change language in UI
// 4. Verify Accept-Language header is sent:
//    - English: "Accept-Language: en"
//    - Georgian: "Accept-Language: ka"  
//    - German: "Accept-Language: de"
// 5. Verify API returns localized data based on header

// Expected API Response Examples:
// English (Accept-Language: en):
// {
//   "name": "Call of Duty: Modern Warfare",
//   "description": "Experience the ultimate first-person shooter..."
// }

// Georgian (Accept-Language: ka):
// {
//   "name": "მოწოდება მოვალეობისა: თანამედროვე ომი", 
//   "description": "გამოცადეთ უკიდურესი პირველი პირის შუტერი..."
// }

// German (Accept-Language: de):
// {
//   "name": "Call of Duty: Modern Warfare",
//   "description": "Erleben Sie den ultimativen Ego-Shooter..."
// }

