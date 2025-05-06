import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ContactSubmission } from '../models/contact-submission.model';
import { environment } from '../../environments/environment';

export interface ContactFormDto {
  name: string;
  email: string;
  subject: string;
  message: string;
}

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  private baseUrl = environment.apiUrl;
  private apiUrl = this.baseUrl= '/Contact';

  constructor(private http: HttpClient) {}

  sendContactForm(data: ContactFormDto): Observable<any> {
    return this.http.post(this.apiUrl, data).pipe(
      catchError(this.handleError)
    );
  }

  getContactById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  getContactSubmissions(): Observable<ContactSubmission[]> {
    const token = localStorage.getItem('admintoken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<ContactSubmission[]>(`${this.apiUrl}/GetContactSubmissionList`, { headers });
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      errorMessage = `Server error ${error.status}: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
