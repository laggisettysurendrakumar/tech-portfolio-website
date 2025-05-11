import { Component, OnInit } from "@angular/core";
import { ThemeService } from "../../core/services/theme.service";

interface Project {
  title: string;
  duration: string;
  client: string;
  role: string;
  type: string;
  technologies: string;
  description: string;
  accomplishments: string[];
  link: string;
  image?: string;
}

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss'],
  standalone:false
})
export class ProjectsComponent implements OnInit {
  isDarkMode: boolean = false;

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.themeService.isDarkMode$.subscribe(mode => {
      this.isDarkMode = mode;
    });
  }

  projects: Project[] = [
    // {
    //   title: 'NCR Voyix Connected Payments',
    //   duration: 'June 2023 – Till now',
    //   client: 'Terafina Software Solutions Pvt Ltd (NCR Voyix)',
    //   role: 'Software Engineer III',
    //   type: 'Web Application',
    //   technologies: '.NET Core 8, WebAPI, Angular 14, TypeScript',
    //   description: `NCR Voyix Connected Payments is the next generation of NCR Counterpoint Gateway to enhance electronic payment processing for NCR merchants. It uses a CCL processing engine as a gateway to process credit, debit, and gift cards.`,
    //   accomplishments: [
    //     'Developed highly scalable web services for secure payment processing.',
    //     'Implemented JWT authentication and role-based access control (RBAC).',
    //     'Conducted code reviews and performance tuning to optimize backend efficiency.',
    //     'Collaborated with cross-functional teams to ensure seamless application deployment.'
    //   ],
    //   link: 'https://example.com/project1'
    // },
    {
      title: 'Mobox',
      duration: 'March 2021 – June 2023',
      client: 'Tech Mahindra',
      role: 'Sr. Software Engineer',
      type: 'Web Application',
      technologies: 'ASP.NET MVC5, Web API, jQuery, Bootstrap',
      description: `MOBOX is the first subscription service pack for premium tires in Europe, offering tire and car maintenance services under a transparent monthly fee with brand options like Bridgestone, Goodyear, and Dunlop.`,
      accomplishments: [
        'Designed and developed responsive web applications for European clients.',
        'Improved database performance by optimizing queries and indexing.',
        'Developed RESTful APIs and integrated them with frontend applications.',
        'Led code reviews and peer programming to improve code quality.',
        'Automated deployment using Azure DevOps CI/CD pipelines.'
      ],
      link: 'https://example.com/project2'
    },
    {
      title: 'My Assignments',
      duration: 'May 2019 – Feb 2021',
      client: 'Accenture',
      role: 'Software Engineer',
      type: 'Web Application',
      technologies: '.NET MVC, jQuery, HTML, Bootstrap, SQL Server',
      description: `MyAssignments is an interim tool for aligning non-master attributes for sales reporting. It supports updates reflecting in downstream systems like the Diamond reporting system.`,
      accomplishments: [
        'Developed and maintained web applications for sales reporting.',
        'Implemented data migration strategies for improved performance.',
        'Conducted root cause analysis (RCA) and debugging to resolve production issues.',
        'Developed dynamic reports using SQL Server Reporting Services (SSRS).'
      ],
      link: 'https://example.com/project3'
    },
    {
      title: 'Reliance Mutual Fund',
      duration: 'Dec 2018 – May 2019',
      client: 'Karvy (Reliance)',
      role: 'Software Engineer',
      type: 'Web Application',
      technologies: 'ASP.NET MVC, MS SQL Server, jQuery, Web Services',
      description: `This project facilitated online transactions in Reliance mutual funds, including portfolio summary, redemption, and purchase of units—even without KYC PAN—through a secure, user-managed interface.`,
      accomplishments: [
        'Developed and optimized financial and e-commerce applications.',
        'Integrated third-party APIs for secure payment processing.',
        'Implemented frontend validations and user authentication mechanisms.',
        'Managed source control and release deployments using SVN and Git.',
        'Provided quick resolutions for critical issues in production during customer testing.'
      ],
      link: 'https://example.com/project4'
    },
    {
      title: 'FFP',
      duration: 'Aug 2015 – Nov 2018',
      client: 'Air Niugini',
      role: 'Software Engineer',
      type: 'Web Application',
      technologies: 'Asp.Net MVC, MS SQL Server with Entity Framework, Web Services, jQuery',
      description: `FFP is a passenger information system supporting loyalty programs, travel history, merchandise management, and integration with CRIS ticketing via Macerator Web Services API.`,
      accomplishments: [
        'Debugged and fixed challenging issues by performing root cause analysis in various modules.',
        'Involved in writing bean classes, BO classes for the End User module.',
        'Created web services as per the application requirement.',
        'Met deadlines for delivering high and immediate priority defects.',
        'Provided quick resolutions for critical issues during production-level customer testing.',
        'Managed SVN code check-in process, including defining and reviewing code.'

      ],
      link: 'https://example.com/project5'
    }
  ];
  
}
