import { Pipe, PipeTransform, inject } from '@angular/core';
import { LocalizationService } from '../services/localization.service';

@Pipe({
  name: 'translate',
  pure: false, // Make it impure to react to language changes
  standalone: true
})
export class TranslatePipe implements PipeTransform {
  private localizationService = inject(LocalizationService);

  transform(key: string): string {
    return this.localizationService.getTranslation(key);
  }
}

