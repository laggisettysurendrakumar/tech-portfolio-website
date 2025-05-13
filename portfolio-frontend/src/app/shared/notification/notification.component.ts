import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';
import { Notification } from '../../models/notification.model';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  standalone: false
})
export class NotificationComponent implements OnInit {
  notification: Notification | null = null;
  visible = false;
  timeoutId: any;

  get alertClass(): string {
    switch (this.notification?.type) {
      case 'success':
        return 'alert-success';
      case 'error':
        return 'alert-danger';
      case 'warning':
        return 'alert-warning';
      default:
        return 'alert-info';
    }
  }

  get customStyle() {
    if (this.notification?.type === 'success') {
      return {
        backgroundColor: '#28a745',
        color: 'white',
        border: '1px solid #1e7e34'
      };
    } else if (this.notification?.type === 'warning') {
      return {
        backgroundColor: '#ffc107', // yellow
        color: '#212529',
        border: '1px solid #d39e00'
      };
    } else if (this.notification?.type === 'error') {
      return {
        backgroundColor: '#dc3545', // red
        color: 'white',
        border: '1px solid #bd2130'
      };
    }
    return {};
  }

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.notificationService.notification$.subscribe((noti) => {
      this.notification = noti;
      this.visible = true;

      clearTimeout(this.timeoutId);
      this.timeoutId = setTimeout(() => {
        this.visible = false;
        setTimeout(() => (this.notification = null), 300);
      }, 2000);
    });
  }
}
