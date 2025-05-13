import { Store } from "@ngrx/store";
import { Reminder } from "../../models/reminder.model";
import { addReminder } from "../../store/reminder/reminder.actions";
import { Component } from "@angular/core";
import { NotificationService } from "../../core/services/notification.service";

@Component({
  selector: 'app-add-reminder',
  templateUrl: './add-reminder.component.html',
  standalone: false
})
export class AddReminderComponent {
  reminder: Reminder = { id: 0, companyName: '', amount: '', description: '', done: false };

  constructor(private store: Store, private notificationService: NotificationService) { }

  onSubmit() {
    this.store.dispatch(addReminder({ reminder: this.reminder }));
    this.reminder = { id: 0, companyName: '', amount: '', description: '', done: false };
    this.notificationService.showSuccess('Reminder added successfully!');
  }
}
