
# SWP391_FBlog-Academy

This system was created to support students of the F University, serving as a platform for writers and readers focused on specific subjects, fields, or courses. Writers can contribute by publishing articles, while readers gain access to quality content related to academic and personal challenges. Additionally, mentors and lecturers play a role in content moderation, and outstanding blog posts receive corresponding honorary rewards based on engagement and feedback.

This is my [Project's Requirement](https://docs.google.com/spreadsheets/d/1QgbaDYhJ-orJ3kNxQvgAo9OHS8cuK-RVBKNzox8sdGY/edit?usp=sharing)


## Directory

```bash
├───.github
├───backend
├───diagram
├───frontend_admin
└───frontend_user
```
## Run Locally

Clone the project

```bash
git clone https://github.com/baomnl123/SWP391_FBlog-Academy.git
```

Go to the project directory

```bash
cd SWP391_FBlog-Academy
```

### For the frontend

Go to the frontend_admin folder

```bash
cd frontend_admin
npm install
```

Or go to the frontend_admin folder

```bash
cd frontend_user
npm install
```

Start the server

```bash
npm run start
```

### For the backend

First, you need to replace the connection string in appsettings.json to connect to your local database

Install dotnet-ef for CLI

```bash
dotnet tool install --global dotnet-ef --version 5.0.17
```

Using Manage Nuget packages to install packages
```bash
Microsoft.Extensions.Configuration -Version 5.0.0
Microsoft.Extensions.Configuration.Json -Version 5.0.0
```

Install package using CLI or Power Shell
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.17
dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.17
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 5.0.17
dotnet add package Microsoft.Data.SqlClient --version 3.0.1
```

Generate database from domain classes – CLI

```bash
dotnet ef database update
```
