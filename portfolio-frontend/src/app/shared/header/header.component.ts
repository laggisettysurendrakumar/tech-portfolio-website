import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: false
})
export class HeaderComponent implements OnInit {
  isDarkMode = false;
  logoImage: string = 'SMLogo.PNG';  

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }
}
