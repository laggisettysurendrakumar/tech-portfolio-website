import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Reminder } from '../../models/reminder.model';

@Injectable
({ providedIn: 'root' })
export class ReminderService {
  private baseUrl = environment.apiUrl;
  private apiUrl = this.baseUrl+'/Reminder';
  

  constructor(private http: HttpClient) {
    console.log('[ReminderService] Constructed');
  }

  getReminders(): Observable<Reminder[]> {
    return this.http.get<Reminder[]>(this.apiUrl+"/GetAllReminderList");
  }

  addReminder(reminder: Reminder): Observable<Reminder> {
    return this.http.post<Reminder>(this.apiUrl+"/AddReminder", reminder);
  }
}
