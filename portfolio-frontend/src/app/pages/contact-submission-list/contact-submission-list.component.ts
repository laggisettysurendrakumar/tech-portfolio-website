import { Component, OnInit } from '@angular/core';
import { ContactSubmission } from '../../models/contact-submission.model';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-submission-list',
  templateUrl: './contact-submission-list.component.html',
  styleUrls: ['./contact-submission-list.component.scss'],
  standalone: false,
})
export class ContactSubmissionListComponent implements OnInit {
  submissions: ContactSubmission[] = [];
  errorMessage: string = '';
  isLoading: boolean = true;

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.loadContactSubmissions();
  }

  loadContactSubmissions(): void {
    this.contactService.getContactSubmissions().subscribe({
      next: (data) => {
        this.submissions = data;
        this.isLoading = false; // Stop loading once data is fetched
      },
      error: (err) => {
        this.isLoading = false; // Stop loading on error
        this.errorMessage = err.error || 'Failed to load contact submissions.';
        console.error('Error loading contact submissions', err); // Optional: Log for debugging
      },
    });
  }
}
