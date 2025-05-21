import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { feedback } from '../../models/feedback.model';
import { environment } from '../../../environments/environment';


@Injectable({ providedIn: 'root' })
export class FeedbackService {

  private baseUrl = environment.apiUrl;
  private apiUrl = this.baseUrl+'/feedback/';


  constructor(private http: HttpClient) {}

  getFeedbacks(): Observable<feedback[]> {
    return this.http.get<feedback[]>(this.apiUrl+'GetFeedbacks');
  }

  submitFeedback(feedback: feedback): Observable<any> {
    return this.http.post(this.apiUrl+'SubmitFeedback', feedback);
  }
}
