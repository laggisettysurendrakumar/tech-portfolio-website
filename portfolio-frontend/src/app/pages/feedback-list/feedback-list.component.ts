import { Component, OnInit } from '@angular/core';
import { FeedbackService } from '../../core/services/feedback.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-feedback-list',
  templateUrl: './feedback-list.component.html',
  styleUrls: ['./feedback-list.component.scss'],
  standalone:false
})
export class FeedbackListComponent implements OnInit {
  feedbacks: any[] = [];
  feedbacksSorted: any[] = [];

  currentSortKey: string = 'index';
  sortAsc: boolean = true;

  constructor(private feedbackService: FeedbackService, private notificationService : NotificationService) {}

  ngOnInit(): void {
    this.feedbackService.getFeedbacks().subscribe({
      next: data => {
        this.feedbacks = data;
        this.sortBy('index');
      },
      error: err => {
        console.log('Failed to fetch feedbacks: ' + err.message);
      this.notificationService.showError('Failed to fetch feedbacks');
    }
    });
  }

  sortBy(key: string): void {
    if (this.currentSortKey === key) {
      this.sortAsc = !this.sortAsc;
    } else {
      this.currentSortKey = key;
      this.sortAsc = true;
    }

    this.feedbacksSorted = [...this.feedbacks].sort((a, b) => {
      if (key === 'index') {
        // Just keep the original order
        return 0;
      }

      let aVal = a[key];
      let bVal = b[key];

      // Handle null/undefined safely
      if (aVal == null) aVal = '';
      if (bVal == null) bVal = '';

      if (typeof aVal === 'string') {
        aVal = aVal.toLowerCase();
        bVal = bVal.toLowerCase();
      }

      if (aVal < bVal) return this.sortAsc ? -1 : 1;
      if (aVal > bVal) return this.sortAsc ? 1 : -1;
      return 0;
    });
  }

  sortIcon(key: string): string {
    if (this.currentSortKey !== key) return 'bi bi-arrow-down-up'; // default icon
    return this.sortAsc ? 'bi bi-arrow-up' : 'bi bi-arrow-down';
  }
}
