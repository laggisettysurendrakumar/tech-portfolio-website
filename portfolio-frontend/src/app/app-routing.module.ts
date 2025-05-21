import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';  
import { AboutComponent } from './pages/about/about.component';  
import { ProjectsComponent } from './pages/projects/projects.component';  
import { ContactComponent } from './pages/contact/contact.component';  
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';
import { ContactSubmissionListComponent } from './pages/contact-submission-list/contact-submission-list.component';
import { AdminAuthGuard } from './core/guards/admin-auth.guard';
import { ReminderListComponent } from './pages/reminder-list/reminder-list.component';
import { AddReminderComponent } from './pages/add-reminder/add-reminder.component';
import { FeedbackFormComponent } from './pages/feedback-form/feedback-form.component';
import { FeedbackListComponent } from './pages/feedback-list/feedback-list.component';

const routes: Routes = [  
  { path: '', component: HomeComponent, data: { title: 'Home | Surendra Kumar Portfolio' } },  
  { path: 'about', component: AboutComponent, data: { title: 'About Me | Surendra Kumar Portfolio' } },  
  { path: 'projects', component: ProjectsComponent, data: { title: 'Projects | Surendra Kumar Portfolio' } },  
  { path: 'contact', component: ContactComponent, data: { title: 'Contact | Surendra Kumar Portfolio' } },  
  { path: 'admin-login', component: AdminLoginComponent, data: { title: 'Admin Login | Surendra Kumar Portfolio' } },  
  { 
    path: 'contact-submissions', 
    component: ContactSubmissionListComponent, 
    canActivate: [AdminAuthGuard],
    data: { title: 'Contact Submissions | Admin | Surendra Kumar Portfolio' } 
  },  
  { 
    path: 'reminder-list', 
    component: ReminderListComponent, 
    canActivate: [AdminAuthGuard],
    data: { title: 'Reminder List | Admin | Surendra Kumar Portfolio' } 
  },  
  { 
    path: 'add-reminder', 
    component: AddReminderComponent, 
    canActivate: [AdminAuthGuard],
    data: { title: 'Add Reminder | Admin | Surendra Kumar Portfolio' } 
  },  
  { path: 'feedback-form', component: FeedbackFormComponent ,
    data: { title: 'Feedback Form | Surendra Kumar Portfolio' } 
  },
  { path: 'feedback-list', component: FeedbackListComponent ,
     canActivate: [AdminAuthGuard],
     data: { title: 'Feedback Form List | Admin | Surendra Kumar Portfolio' } 
  },
  
  { path: '**', redirectTo: '', pathMatch: 'full' }  // Fallback for undefined routes
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
