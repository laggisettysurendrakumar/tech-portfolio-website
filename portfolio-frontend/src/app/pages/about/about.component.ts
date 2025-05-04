import { Component, OnInit } from '@angular/core';  
import { ThemeService } from '../../services/theme.service';

@Component({  
  selector: 'app-about',  
  templateUrl: './about.component.html',  
  styleUrls: ['./about.component.scss'] ,
  standalone:false 
})  
export class AboutComponent implements OnInit{  

  constructor(private themeService: ThemeService) {}
  profileImage: string = '/user.jpg';  
  isDarkMode = false;

  // Array of skills with name and icon class for Bootstrap Icons  
  skills = [  
    { name: 'Angular', icon: 'bi bi-angular' },  
    { name: 'Bootstrap', icon: 'bi bi-bootstrap' },  
    { name: 'HTML5', icon: 'bi bi-filetype-html' },  
    { name: 'CSS3', icon: 'bi bi-filetype-css' },  
    { name: 'JavaScript', icon: 'bi bi-filetype-js' },  
    { name: 'Git', icon: 'bi bi-git' }  
  ];  

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;
    });
  }
}