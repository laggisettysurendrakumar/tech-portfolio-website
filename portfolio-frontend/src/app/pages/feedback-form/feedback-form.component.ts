import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FeedbackService } from '../../core/services/feedback.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-feedback-form',
  templateUrl: './feedback-form.component.html',
  standalone:false
})
export class FeedbackFormComponent implements OnInit {
  feedbackForm!: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private service: FeedbackService, private notificationService : NotificationService) {}

  ngOnInit(): void {
    this.feedbackForm = this.fb.group({
      relationship: ['', Validators.required],
      communicationRating: [null, [Validators.required, Validators.min(1), Validators.max(5)]],
      collaborationRating: [null, [Validators.required, Validators.min(1), Validators.max(5)]],
      technicalSkillRating: [null, [Validators.required, Validators.min(1), Validators.max(5)]],
      codeQualityRating: [null, [Validators.required, Validators.min(1), Validators.max(5)]],
      helpfulnessRating: [null, [Validators.required, Validators.min(1), Validators.max(5)]],
      whatWentWell: [''],
      whatCouldBeImproved: [''],
      farewellNote: [''],
      yourName: ['', Validators.required]
    });
  }

  get f() {
    return this.feedbackForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    
    if (this.feedbackForm.invalid) 
      return;

    this.service.submitFeedback(this.feedbackForm.value).subscribe({
      next: () => {

        this.notificationService.showSuccess('Feedback submitted!');
        this.feedbackForm.reset();
        this.submitted = false;
      },
      error: () => this.notificationService.showError('Feedback submission is failed.') 
    });
  }

  getLabel(control: string): string {
    const labels: any = {
      communicationRating: 'Communication (1–5)',
      collaborationRating: 'Collaboration (1–5)',
      technicalSkillRating: 'Technical Skill (1–5)',
      codeQualityRating: 'Code Quality (1–5)',
      helpfulnessRating: 'Helpfulness (1–5)',
    };
    return labels[control] || control;
  }
}
