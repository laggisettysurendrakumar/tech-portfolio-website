# 💼 Surendra's Tech Portfolio

Welcome to my personal tech portfolio! 

🙋‍♂️ About Me
I'm Surendra, a passionate full-stack developer focused on building clean, scalable, and user-centric applications.

Check out more on my LinkedIn( https://www.linkedin.com/in/surendrakumarlaggisetty/ ) or reach out at: surilsk4@gmail.com

This project showcases my skills, projects, and background as a full-stack developer using Angular 19 for the frontend and ASP.NET Core WebAPI for the backend.

## 🚀 Tech Stack

**Frontend:**
- Angular 19
- TypeScript
- SCSS / Bootstrap / (your choice)

**Backend:**
- ASP.NET Core WebAPI (.NET 8)
- C#
- RESTful APIs
- Entity FrameWork Core
- MS SQL Server
- SQLLite(optional)

**3rd Party Integrations**
- Implemented **SendGrid** integration for transactional and contact form email delivery with secure API key management.
- Integrated **Twilio SMS service** for real-time user notifications and two-factor authentication (2FA).

**Other Tools:**
- Visual Studio / VS Code
- Git & GitHub
- CI/CD using GitHub Actions
- Node.js / npm
- SQL Server / EF Core 
- Docker (optional)


## 📁 Project Structure

/tech-portfolio-website

│

├── /portfolio-frontend        # Angular 19 Frontend

│   ├── /public/

│   └── /src/

│       ├── /app/

│       │   ├── /core/

│       │   │   ├── /guards/

│       │   │   ├── /interceptors/

│       │   │   └── /services/

│       │   ├── /models/

│       │   ├── /pages/

│       │   ├── /shared/

│       │   └── /store/

│       └── /environments/

│

├── /PortfolioAPI              # ASP.NET Core MVC Backend

│   ├── /Controllers/

│   ├── /Data/

│   ├── /Encryption/

│   ├── /HostedService/

│   ├── /Middlewares/

│   ├── /Migrations/

│   ├── /Models/

│   ├── /Properties/

│   ├── /Resources/

│   ├── /Services/

│   └── /wwwroot/

│

├── README.md

├── .gitignore

└── LICENSE (optional)



## 🧪 How to Run the Project

### 1. Clone the Repository
bash git clone https://github.com/laggisettysurendrakumar/tech-portfolio-website.git

cd tech-portfolio-website

### 2. Run the Backend (ASP.NET Core)

cd PortfolioAPI

dotnet restore

dotnet run

Backend will run on: https://localhost:5001 (Port will be based on your system)

### 3. Run the Frontend (Angular 19)

cd portfolio-frontend

npm install

ng serve

## 🔄 Clean Build (No Cache) — Windows

If you're experiencing dependency injection or stale build issues, use the following commands to perform a clean rebuild on Windows:

```bash
rd /s /q dist
rd /s /q node_modules
npm cache clean --force
npm install
npx ng serve --no-cache
```
This will:
Delete previous build output and dependencies
Clear npm cache
Reinstall fresh dependencies
Start the dev server without Angular's internal cache


## 🔄 Prod Build 
To build your Angular project for production, you can use the following command:
```bash
ng build --configuration production
```
Note: It will create the dist folder on the Angular root folder and it will place publised files with application name
Frontend will run on: http://localhost:4200 (Port will be based on your system)

Note : Make sure the API base URL in your Angular services matches your backend URL.

🌐 Live Demo

Surendra Portfolio website : ( https://www.surendraportfolio.somee.com )

Or deploy it using:

Azure App Service / Vercel / Netlify / GitHub Pages (frontend)

Azure Web App / Render / Railway (backend)
