import { Injectable, signal } from '@angular/core';
import { EN_TRANSLATIONS, TranslationKey } from '../../i18n/en';
import { AR_TRANSLATIONS } from '../../i18n/ar';

export type Language = 'en' | 'ar';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  private currentLanguage = signal<Language>('ar'); // Default to Arabic
  private translations: Record<Language, Record<TranslationKey, string>> = {
    en: EN_TRANSLATIONS,
    ar: AR_TRANSLATIONS
  };

  constructor() {
    // Load language from localStorage if available (only in browser)
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      const savedLanguage = localStorage.getItem('language') as Language;
      if (savedLanguage && (savedLanguage === 'en' || savedLanguage === 'ar')) {
        this.currentLanguage.set(savedLanguage);
      }
    }
    
    // Apply RTL/LTR direction
    this.updateDirection();
  }

  /**
   * Get current language
   */
  getCurrentLanguage(): Language {
    return this.currentLanguage();
  }

  /**
   * Get current language as signal for reactive updates
   */
  getCurrentLanguageSignal() {
    return this.currentLanguage.asReadonly();
  }

  /**
   * Set language and persist to localStorage
   */
  setLanguage(language: Language): void {
    this.currentLanguage.set(language);
    // Only save to localStorage in browser environment
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      localStorage.setItem('language', language);
    }
    this.updateDirection();
  }

  /**
   * Toggle between English and Arabic
   */
  toggleLanguage(): void {
    const newLanguage = this.currentLanguage() === 'en' ? 'ar' : 'en';
    this.setLanguage(newLanguage);
  }

  /**
   * Get translation for a key
   */
  translate(key: TranslationKey): string {
    const currentLang = this.currentLanguage();
    return this.translations[currentLang][key] || key;
  }

  /**
   * Get translation for a key with fallback
   */
  t(key: TranslationKey, fallback?: string): string {
    const translation = this.translate(key);
    return translation !== key ? translation : (fallback || key);
  }

  /**
   * Check if current language is RTL
   */
  isRTL(): boolean {
    return this.currentLanguage() === 'ar';
  }

  /**
   * Check if current language is LTR
   */
  isLTR(): boolean {
    return this.currentLanguage() === 'en';
  }

  /**
   * Get current language direction
   */
  getDirection(): 'rtl' | 'ltr' {
    return this.isRTL() ? 'rtl' : 'ltr';
  }

  /**
   * Update document direction and lang attribute
   */
  private updateDirection(): void {
    // Only update DOM in browser environment
    if (typeof document !== 'undefined') {
      const html = document.documentElement;
      const direction = this.getDirection();
      const language = this.currentLanguage();
      
      html.setAttribute('dir', direction);
      html.setAttribute('lang', language);
      
      // Add language class to body for CSS styling
      if (document.body) {
        document.body.className = document.body.className.replace(/\b(lang-en|lang-ar)\b/g, '');
        document.body.classList.add(`lang-${language}`);
      }
    }
  }

  /**
   * Get classification options in current language
   */
  getClassificationOptions(): { value: string; label: string }[] {
    return [
      {
        value: this.translate('classification.less_than_3m'),
        label: this.translate('classification.less_than_3m')
      },
      {
        value: this.translate('classification.more_than_3m'),
        label: this.translate('classification.more_than_3m')
      }
    ];
  }
}