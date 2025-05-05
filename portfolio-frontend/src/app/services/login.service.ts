import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private tokenKey = 'admintoken';
  private apiUrl = 'https://localhost:7056/api/auth/admin-login';

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  isLoggedIn$ = this.isLoggedInSubject.asObservable(); // expose observable for other components

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(this.apiUrl, { email, password });
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isLoggedInSubject.next(false); // notify subscribers
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    this.isLoggedInSubject.next(true); // notify subscribers
  }

  notifyLoginStatus(): void {
    this.isLoggedInSubject.next(this.isLoggedIn());
  }
}
