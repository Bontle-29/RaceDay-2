# RaceDay RESTful API — Part 2

**Module:** PROG6212
**Student number:** ST10454360
**Project:** RaceDay RESTful API

## 1. Project description

RaceDay is a RESTful API for managing road-race events. The system allows public users to view published events and allows registered participants, organisers, and administrators to perform actions according to their roles.

The API manages:

- User registration and login

- User profiles

- Race events

- Race categories

- Participant enrolments

- Race results

- Event routes

- Weather forecasts

## 2. Technologies used

- ASP.NET Core Web API

- C#

- Entity Framework Core

- SQL Server / LocalDB

- JWT Bearer authentication

- Role-based authorization

- Swagger/OpenAPI

- xUnit automated testing

- GitHub Actions CI/CD

## 3. User roles

| Role | Permissions |
| --- | --- |
| Public | View published events, categories, routes, weather information, and published results |
| Participant | Manage own profile, enrol in categories, cancel own enrolments, and view own results |
| Organiser | Create and manage owned events, categories, enrolments, routes, weather information, and results |
| Admin | Manage and view all users and system resources |

## 4. Project structure

```
RaceDay
├── RaceDay.Api
│   ├── Controllers
│   ├── Data
│   ├── DTOs
│   ├── Entities
│   ├── Migrations
│   ├── Program.cs
│   └── appsettings.json
│
├── RaceDay.Tests
│   ├── AuthTests.cs
│   ├── LoginTests.cs
│   ├── EventTests.cs
│   ├── EnrolmentTests.cs
│   └── CustomWebApplicationFactory.cs
│
├── .github
│   └── workflows
│       └── dotnet-ci.yml
│
├── README.md
└── RaceDay.sln
```

## 5. How to run the project

### Requirements

Install the following:

- Visual Studio with ASP.NET and web development workload

- .NET SDK

- SQL Server LocalDB or SQL Server

- Git

### Clone the repository

```bash
git clone https://github.com/YOUR-USERNAME/RaceDay-Part2.git
cd RaceDay-Part2
```

Replace `YOUR-USERNAME` with the GitHub username that owns the repository.

### Configure the database

Update the connection string in `RaceDay.Api/appsettings.json` if necessary:

```json
"ConnectionStrings": {
  "RaceDayConnection": "Server=(localdb )\\MSSQLLocalDB;Database=RaceDayDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Apply the database migration

From Visual Studio Package Manager Console:

```
Update-Database
```

Or from a terminal:

```bash
dotnet ef database update --project RaceDay.Api
```

### Run the API

```bash
dotnet run --project RaceDay.Api
```

Open Swagger using the URL shown in the terminal, for example:

```
https://localhost:7232/swagger
```

The port number may be different on another computer.

## 6. Authentication and authorization

The API uses JWT Bearer tokens.

1. Register a participant or organiser using:

```
POST /api/auth/register
```

1. Log in using:

```
POST /api/auth/login
```

1. Copy the returned token.

1. Click the **Authorize** button in Swagger.

1. Enter the token in this format:

```
Bearer YOUR-TOKEN-HERE
```

Passwords are hashed before they are stored. Passwords and password hashes are not returned in API responses.

## 7. Main API endpoints

### Authentication

```
POST /api/auth/register
POST /api/auth/login
```

### User profiles

```
GET /api/users/me
PUT /api/users/me
```

### Events

```
GET    /api/events
GET    /api/events/{eventId}
POST   /api/events
PUT    /api/events/{eventId}
DELETE /api/events/{eventId}
```

### Categories

```
GET    /api/events/{eventId}/categories
GET    /api/categories/{categoryId}
POST   /api/events/{eventId}/categories
PUT    /api/categories/{categoryId}
DELETE /api/categories/{categoryId}
```

### Enrolments

```
GET  /api/enrolments/me
POST /api/categories/{categoryId}/enrolments
PUT  /api/enrolments/{enrolmentId}/cancel
```

### Results

```
GET    /api/events/{eventId}/results
GET    /api/results/{resultId}
POST   /api/enrolments/{enrolmentId}/result
DELETE /api/results/{resultId}
```

### Routes and weather

```
GET  /api/events/{eventId}/route
PUT  /api/events/{eventId}/route
GET  /api/events/{eventId}/weather
POST /api/events/{eventId}/weather
```

## 8. Running the tests

Run all automated tests from the solution folder:

```bash
dotnet test
```

The tests cover:

- Successful user registration

- Invalid login details

- Authentication requirements

- Role-based access control

- Organiser event management

- Participant enrolment

- Duplicate enrolment prevention

Expected result:

```
Failed: 0
```

## 9. GitHub Actions CI/CD

GitHub Actions automatically restores, builds, and tests the project whenever code is pushed to the repository.

Workflow file:

```
.github/workflows/dotnet-ci.yml
```

### Successful CI/CD run

Insert a screenshot of the successful green GitHub Actions workflow below:

![Successful GitHub Actions build](docs/github-actions-success.png)

If you do not have a `docs` folder, create one and copy your screenshot into it using this filename:

```
github-actions-success.png
```

## 10. Evidence screenshots

The following evidence demonstrates the implementation:

- Swagger registration and login

- JWT authorization in Swagger

- Organiser creating an event

- Participant being denied organiser functionality

- Participant enrolment

- Automated tests passing

- Successful GitHub Actions workflow

Add screenshots to the repository if required by the submission instructions.

## 11. Demonstration video

Unlisted YouTube demonstration:

```
PASTE-YOUR-UNLISTED-YOUTUBE-LINK-HERE
```

The video demonstrates the project structure, authentication, role-based authorization, Swagger endpoints, enrolments, results, automated tests, and GitHub Actions.

## 12. Security notes

- Passwords are hashed and are never stored in their original form.

- Password hashes are not returned by API responses.

- Private endpoints require authentication.

- Organiser operations require the Organiser or Admin role.

- Ownership checks prevent an organiser from editing another organiser's event.

- Development secrets should not be committed to a public repository.

## 13. Submission information

**GitHub repository:**

```
PASTE-YOUR-GITHUB-REPOSITORY-LINK-HERE
```

**Student number:** ST10454360
**Module:** PROG6212
**Part:** Part 2 — RESTful API Development
