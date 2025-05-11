import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';
import { CardComponent } from './shared/card/card.component';
import { HomeComponent } from './pages/home/home.component';
import { AboutComponent } from './pages/about/about.component';
import { ProjectsComponent } from './pages/projects/projects.component';
import { ContactComponent } from './pages/contact/contact.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';
import { ContactSubmissionListComponent } from './pages/contact-submission-list/contact-submission-list.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AddReminderComponent } from './pages/add-reminder/add-reminder.component';
import { ReminderListComponent } from './pages/reminder-list/reminder-list.component';
import { reminderReducer } from './store/reminder/reminder.reducer';
import { ReminderEffects } from './store/reminder/reminder.effects';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    CardComponent,
    HomeComponent,
    AboutComponent,
    ProjectsComponent,
    ContactComponent,
    AdminLoginComponent,
    ContactSubmissionListComponent,
    AddReminderComponent,
    ReminderListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    StoreModule.forRoot({ reminders: reminderReducer }),  
    EffectsModule.forRoot([ReminderEffects]),  
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: true })
],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true, // This allows multiple interceptors
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
