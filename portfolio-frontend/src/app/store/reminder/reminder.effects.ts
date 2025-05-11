import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as ReminderActions from './reminder.actions';
import { catchError, map, mergeMap, of } from 'rxjs';
import { ReminderService } from '../../core/services/reminder.service'; // ✅ Ensure correct path

@Injectable()
export class ReminderEffects {
  loadReminders$;
  addReminder$;

  constructor(private actions$: Actions, private service: ReminderService) {
    console.log('[ReminderEffects] Constructed:', !!service);

    this.loadReminders$ = createEffect(() =>
      this.actions$.pipe(
        ofType(ReminderActions.loadReminders),
        mergeMap(() =>
          this.service.getReminders().pipe(
            map(reminders => ReminderActions.loadRemindersSuccess({ reminders })),
            catchError(() => of({ type: '[Reminder] Load Failed' }))
          )
        )
      )
    );

    this.addReminder$ = createEffect(() =>
      this.actions$.pipe(
        ofType(ReminderActions.addReminder),
        mergeMap(action =>
          this.service.addReminder(action.reminder).pipe(
            map(reminder => ReminderActions.addReminderSuccess({ reminder })),
            catchError(() => of({ type: '[Reminder] Add Failed' }))
          )
        )
      )
    );
  }
}
