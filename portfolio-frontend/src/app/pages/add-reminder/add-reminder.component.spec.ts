import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddReminderComponent } from './add-reminder.component';

describe('AddReminderComponent', () => {
  let component: AddReminderComponent;
  let fixture: ComponentFixture<AddReminderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddReminderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddReminderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
