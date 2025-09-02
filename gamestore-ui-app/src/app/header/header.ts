import { Component, HostListener } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

interface Language {
  code: string;
  label: string;
}

@Component({
  selector: 'app-header',
  imports: [RouterLink, CommonModule, TranslateModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  isDropdownOpen = false;
  
  languages: Language[] = [
    { code: 'en', label: 'ENG' },
    { code: 'geo', label: 'GEO' },
    { code: 'de', label: 'DE' }
  ];
  
  selectedLanguage: Language = this.languages[0]; // Default to ENG

  constructor(private translate: TranslateService) {
    // Set default language
    this.translate.setDefaultLang('en');
    this.translate.use('en');
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  selectLanguage(language: Language): void {
    this.selectedLanguage = language;
    this.isDropdownOpen = false;
    
    // Change the application language
    this.translate.use(language.code);
    console.log('Language changed to:', language.code);
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    const dropdown = target.closest('.language-dropdown');
    
    if (!dropdown && this.isDropdownOpen) {
      this.isDropdownOpen = false;
    }
  }
}
