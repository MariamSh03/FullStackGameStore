import { Component, computed, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LocalizationService, Language } from '../services/localization.service';
import { TranslatePipe } from '../pipes/translate.pipe';

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule, TranslatePipe],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  public isLanguageDropdownOpen = signal(false);
  
  constructor(public localizationService: LocalizationService) {}

  public toggleLanguageDropdown(): void {
    this.isLanguageDropdownOpen.update(value => !value);
  }

  public selectLanguage(language: Language): void {
    this.localizationService.setLanguage(language.code);
    this.isLanguageDropdownOpen.set(false);
  }

  // Close dropdown when clicking outside
  public onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.language-selector')) {
      this.isLanguageDropdownOpen.set(false);
    }
  }
}
