import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactFormDto, ContactService } from '../../services/contact.service';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  standalone:false
})
export class ContactComponent implements OnInit {
  contactForm: FormGroup;
  submitted = false;
  responseMessage = '';
  errorMessage = '';
  isDarkMode: boolean = false;

  constructor(private fb: FormBuilder, private contactService: ContactService, private themeService: ThemeService) {
    this.contactForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      subject: ['', Validators.required],
      message: ['', Validators.required]
    });
  }

ngOnInit() {
  this.themeService.isDarkMode$.subscribe(mode => {
    this.isDarkMode = mode;
  });
}

  onSubmit(): void {
    this.submitted = true;
    this.responseMessage = '';
    this.errorMessage = '';

    if (this.contactForm.invalid) {
      return;
    }

    const formData: ContactFormDto = this.contactForm.value;

    this.contactService.sendContactForm(formData).subscribe({
      next: (response) => {
        this.responseMessage = 'Your message has been sent successfully!';
        this.contactForm.reset();
        this.submitted = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Something went wrong.';
      }
    });
  }
}
