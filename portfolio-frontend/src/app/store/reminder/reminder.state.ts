import { Reminder } from "../../models/reminder.model";

export const reminderFeatureKey = 'reminders';

export interface ReminderState {
  reminders: Reminder[];
}

export const initialReminderState: ReminderState = {
  reminders: [],
};
