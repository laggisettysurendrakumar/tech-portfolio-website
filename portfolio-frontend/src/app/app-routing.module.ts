import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';  
import { AboutComponent } from './pages/about/about.component';  
import { ProjectsComponent } from './pages/projects/projects.component';  
import { ContactComponent } from './pages/contact/contact.component';  
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';
import { ContactSubmissionListComponent } from './pages/contact-submission-list/contact-submission-list.component';
import { AdminAuthGuard } from './guards/admin-auth.guard';

const routes: Routes = [  
  { path: '', component: HomeComponent }, // default route (home page)  
  { path: 'about', component: AboutComponent },  
  { path: 'projects', component: ProjectsComponent },  
  { path: 'contact', component: ContactComponent },  
  { path: 'adminlogin', component: AdminLoginComponent },  
  {
    path: 'contact-submissions',
    component: ContactSubmissionListComponent,
    canActivate: [AdminAuthGuard] // ✅ Guard applied here
  },
  
  // Optional: add a wildcard route for 404 page  
  // { path: '**', redirectTo: '', pathMatch: 'full' }  
];  

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
