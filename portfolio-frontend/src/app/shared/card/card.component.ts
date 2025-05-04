import { Component, Input } from '@angular/core';  

export interface Project {  
  title: string;  
  description: string;  
  image: string;  
  link: string;  
}  

@Component({  
  selector: 'app-card',  
  templateUrl: './card.component.html',  
  styleUrls: ['./card.component.scss'] ,
  standalone:false 
})  
export class CardComponent {  
  @Input() project!: Project;  
}  