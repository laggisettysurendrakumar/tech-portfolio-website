import { Component } from '@angular/core';
import { Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.scss'],
  standalone:false
})
export class AdminLoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  loading: boolean = false;

  private encryptionKey = 'YourSharedSecretKey123'; // Keep in sync with backend

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}


   generateRandomKeyIV(): { key: string; iv: string } {
    const key = CryptoJS.lib.WordArray.random(32); // 256-bit key
    const iv = CryptoJS.lib.WordArray.random(16);  // 128-bit IV
    return {
      key: CryptoJS.enc.Base64.stringify(key),
      iv: CryptoJS.enc.Base64.stringify(iv)
    };
  }

  encryptPassword(password: string, keyBase64: string, ivBase64: string): string {
    const key = CryptoJS.enc.Base64.parse(keyBase64);
    const iv = CryptoJS.enc.Base64.parse(ivBase64);
    const encrypted = CryptoJS.AES.encrypt(password, key, {
      iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    return encrypted.ciphertext.toString(CryptoJS.enc.Base64);
  }

  onLogin(): void {
    this.errorMessage = '';
    this.loading = true;
    const keyBase64 =  'mZp9cUHQaYF0wAnJSF6xAx/9v2+/ZkBGNz4H1z5HezY='; //this.generateRandomKeyIV().key;
    const ivBase64 = 'MTIzNDU2Nzg5MGFiY2RlZg=='; // "testIVforASP" (example only — use 16-byte values) //this.generateRandomKeyIV().iv;
    
    const encryptedPassword = this.encryptPassword(this.password,keyBase64,ivBase64);

    this.loginService.login(this.email, encryptedPassword).subscribe({
      next: (response) => {
        localStorage.setItem('admintoken', response.token);
        // ✅ Notify app of updated login status
        this.loginService.notifyLoginStatus();
        this.router.navigate(['/contact-submissions']);
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Login failed';
        this.loading = false;
      }
    });
  }
}
