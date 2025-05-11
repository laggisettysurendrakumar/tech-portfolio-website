import { Component, OnInit } from '@angular/core';
import { Reminder } from '../../models/reminder.model';
import { Store } from '@ngrx/store';
import { loadReminders } from '../../store/reminder/reminder.actions';
import { selectAllReminders } from '../../store/reminder/reminder.selectors';

@Component({
  selector: 'app-reminder-list',
  templateUrl: './reminder-list.component.html',
  standalone: false
})
export class ReminderListComponent implements OnInit {
  reminders: Reminder[] = [];
  filteredReminders: Reminder[] | undefined = [];
  sortOrder: string = 'all';

  constructor(private store: Store) {}

  ngOnInit() {
    this.store.dispatch(loadReminders());
    this.store.select(selectAllReminders).subscribe((reminders: Reminder[]) => {
      this.reminders = reminders;
      this.sortReminders();
    });
  }

  sortReminders() {
    this.filteredReminders = {
      'done': this.reminders.filter(r => r.done),
      'not-done': this.reminders.filter(r => !r.done),
      'all': this.reminders
    }[this.sortOrder];
  }

  exportToExcel() {
    // Excel logic (uncomment when ready)
  }
}
