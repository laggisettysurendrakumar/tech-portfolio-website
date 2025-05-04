import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';  
import { AboutComponent } from './pages/about/about.component';  
import { ProjectsComponent } from './pages/projects/projects.component';  
import { ContactComponent } from './pages/contact/contact.component';  

const routes: Routes = [  
  { path: '', component: HomeComponent }, // default route (home page)  
  { path: 'about', component: AboutComponent },  
  { path: 'projects', component: ProjectsComponent },  
  { path: 'contact', component: ContactComponent },  
  // Optional: add a wildcard route for 404 page  
  // { path: '**', redirectTo: '', pathMatch: 'full' }  
];  

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
