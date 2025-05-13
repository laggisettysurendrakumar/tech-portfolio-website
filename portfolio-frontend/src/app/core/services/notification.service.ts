import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Notification } from '../../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationSubject = new Subject<Notification>();
  notification$ = this.notificationSubject.asObservable();

  showSuccess(message: string) {
    this.show(message, 'success');
  }

  showError(message: string) {
    this.show(message, 'error');
  }

  showWarning(message: string) {
    this.show(message, 'warning');
  }

  private show(message: string, type: Notification['type']) {
    this.notificationSubject.next({ message, type });
  }
}
