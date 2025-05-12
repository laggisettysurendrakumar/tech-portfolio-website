import { createAction, props } from '@ngrx/store';
import { Reminder } from '../../models/reminder.model';

export const loadReminders = createAction('[Reminder] Load');
export const loadRemindersSuccess = createAction('[Reminder] Load Success', props<{ reminders: Reminder[] }>());

export const addReminder = createAction('[Reminder] Add', props<{ reminder: Reminder }>());
export const addReminderSuccess = createAction('[Reminder] Add Success', props<{ reminder: Reminder }>());

export const updateReminder = createAction(
  '[Reminder] Update Reminder',
  props<{ reminder: Reminder }>()
);

export const updateReminderSuccess = createAction(
  '[Reminder] Update Reminder Success',
  props<{ reminder: Reminder }>()
);