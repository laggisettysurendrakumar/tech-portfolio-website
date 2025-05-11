import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ReminderState } from './reminder.state';

export const selectReminderState = createFeatureSelector<ReminderState>('reminders');

export const selectAllReminders = createSelector(
  selectReminderState,
  (state: ReminderState) => state.reminders
);
