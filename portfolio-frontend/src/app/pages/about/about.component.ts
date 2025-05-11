import { Component, OnInit } from '@angular/core';  
import { ThemeService } from '../../core/services/theme.service';


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
    { name: 'Angular', icon: 'fa-brands fa-angular  fa-flip', color: '#dd1b16' },
    { name: 'ASP.NET Core', icon: 'fa fa-code fa-beat', color: '#512bd4' },
    { name: 'C#', icon: 'fa fa-code fa-beat', color: '#68217a' }, // custom icon or font if available
    { name: 'Microsoft SQL Server', icon: 'fa fa-database fa-beat', color: '#00758f' },
    { name: 'Entity Framework', icon: 'fa fa-database fa-beat', color: '#68217a' },
    { name: 'REST APIs', icon: 'fa fa-cogs fa-beat', color: '#00bfa6' },
    { name: 'Bootstrap', icon: 'bi bi-bootstrap', color: '#7952b3' },
    { name: 'HTML5', icon: 'bi bi-filetype-html', color: '#e34c26' },
    { name: 'CSS3', icon: 'bi bi-filetype-css', color: '#264de4' },
    { name: 'JavaScript', icon: 'bi bi-filetype-js', color: '#f0db4f' },
    { name: 'TypeScript', icon: 'bi bi-code', color: '#3178c6' },
    { name: 'Git', icon: 'bi bi-git', color: '#f1502f' }
  ];

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;
    });
  }
}