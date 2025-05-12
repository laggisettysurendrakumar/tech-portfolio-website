import { createReducer, on } from '@ngrx/store';
import { ReminderState, initialReminderState } from './reminder.state';
import * as ReminderActions from './reminder.actions';

export const reminderReducer = createReducer(
  initialReminderState,
  on(ReminderActions.loadRemindersSuccess, (state, { reminders }) => ({
    ...state,
    reminders,
  })),
  on(ReminderActions.addReminderSuccess, (state, { reminder }) => ({
    ...state,
    reminders: [...state.reminders, reminder],
  })),
  on(ReminderActions.updateReminderSuccess, (state, { reminder }) => ({
  ...state,
  reminders: state.reminders.map(r => r.id === reminder.id ? reminder : r)
}))
);
