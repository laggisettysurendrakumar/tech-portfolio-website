import { createAction, props } from '@ngrx/store';
import { Reminder } from '../../models/reminder.model';

export const loadReminders = createAction('[Reminder] Load');
export const loadRemindersSuccess = createAction('[Reminder] Load Success', props<{ reminders: Reminder[] }>());

export const addReminder = createAction('[Reminder] Add', props<{ reminder: Reminder }>());
export const addReminderSuccess = createAction('[Reminder] Add Success', props<{ reminder: Reminder }>());
