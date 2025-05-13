import { Router } from "@angular/router";
import { LoginService } from "../../core/services/login.service";
import { ThemeService } from "../../core/services/theme.service";
import { Component, OnInit } from "@angular/core";
import { NotificationService } from "../../core/services/notification.service";

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  isDarkMode = false;
  loginStatusText: string = 'Login';
  isAdmin: boolean = false;
  logoImage: string = 'SMLogo.PNG';

  constructor(
    private themeService: ThemeService,
    private loginService: LoginService,
    private router: Router,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;
    });

    this.loginService.isLoggedIn$.subscribe(isLoggedIn => {
      this.loginStatusText = isLoggedIn ? 'Logout' : 'Login';
      this.setAdminStatus();
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  toggleLoginStatus(): void {
    if (this.loginService.isLoggedIn()) {
      this.loginService.logout();
      this.notificationService.showSuccess("Logged out successfully!");
      this.router.navigate(['/admin-login']);
    } else {
      this.router.navigate(['/admin-login']);
    }
    this.loginService.notifyLoginStatus();
  }

  setAdminStatus(): void {
    const role = localStorage.getItem('role');
    this.isAdmin = role === 'admin';
  }
}
