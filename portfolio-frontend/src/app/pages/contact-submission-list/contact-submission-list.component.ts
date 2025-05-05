// contact-submission-list.component.ts
import { Component, OnInit } from '@angular/core';
import { ContactSubmission } from '../../models/contact-submission.model';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-submission-list',
  templateUrl: './contact-submission-list.component.html',
  styleUrls: ['./contact-submission-list.component.scss'],
  standalone:false
})
export class ContactSubmissionListComponent implements OnInit {
  submissions: ContactSubmission[] = [];
  errorMessage: string = '';

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.contactService.getContactSubmissions().subscribe({
      next: (data) => this.submissions = data,
      error: (err) => this.errorMessage = err.error || 'Failed to load contact submissions.'
    });
  }
}
