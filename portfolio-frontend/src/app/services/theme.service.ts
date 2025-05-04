import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private darkModeSubject = new BehaviorSubject<boolean>(false);
  isDarkMode$ = this.darkModeSubject.asObservable();

  constructor() {
    this.loadTheme();
  }

  toggleTheme(): void {
    const isDark = !this.darkModeSubject.value;
    this.setDarkMode(isDark);
  }

  setDarkMode(isDark: boolean): void {
    const theme = isDark ? 'dark-theme' : 'light-theme';
    this.darkModeSubject.next(isDark);
    document.body.setAttribute('class', theme);
    localStorage.setItem('theme', theme);
  }

  loadTheme(): void {
    const savedTheme = localStorage.getItem('theme') || 'light-theme';
    const isDark = savedTheme === 'dark-theme';
    this.darkModeSubject.next(isDark);
    document.body.setAttribute('class', savedTheme);
  }
}
