import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../services/theme.service';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  standalone: false
})
export class HeaderComponent implements OnInit {
  isDarkMode = false;
  logoImage: string = 'SMLogo.PNG'; 
  loginStatusText:string ='LogIn'; 

  constructor(private themeService: ThemeService, private loginService: LoginService ,private router: Router) {}

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;      
    });
    this.loginService.isLoggedIn$.subscribe(isLoggedIn => {
      this.loginStatusText = isLoggedIn ? 'Logout' : 'Login';
    });
    
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  toggleLoginStatus(): void {
    if (this.loginService.isLoggedIn()) {
      this.loginService.logout();
      this.router.navigate(['/adminlogin']);
    } else {
      this.router.navigate(['/adminlogin']);
    }
  
    this.loginService.notifyLoginStatus(); // cleaner, more encapsulated
  }

}
