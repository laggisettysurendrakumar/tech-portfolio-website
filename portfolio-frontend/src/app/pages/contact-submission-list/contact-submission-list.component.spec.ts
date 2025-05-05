import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactSubmissionListComponent } from './contact-submission-list.component';

describe('ContactSubmissionListComponent', () => {
  let component: ContactSubmissionListComponent;
  let fixture: ComponentFixture<ContactSubmissionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ContactSubmissionListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContactSubmissionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
