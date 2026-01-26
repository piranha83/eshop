# Docker

Run `docker compose up` for for a prod server(moq) and client. Navigate Eshop client to `http://localhost:8083/`.

# Moq service
This moq server was generated with [json-server](https://hub.docker.com/r/clue/json-server).

# Catalog api service

This catalog project was generated with [dotnet core].

### Development

Run `dotnet run` for a dev server. Navigate to  `https://localhost:7259/openapi/v1.json` or `http://localhost:3000/products`.

### Add migrations

Run `dotnet ef migrations add [MigrationName] --output-dir DatabaseContext/Migrations' for add new migration.
Run `dotnet ef database update --context ApplicationDbContext --connection "[ConnectionString]"' for updating db.

# Eshop client

This frontend project was generated with [Angular CLI](https://github.com/angular/angular-cli).

### Install

Run `npm i`.

### Development

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

### Production

Run `ng serve --configuration production` for a prod server. Navigate to `http://localhost:4200/`.

### Deploy on github

Run `ng run deploy` for a github server. Navigate to `https://piranha83.github.io/eshop`.

### Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

### Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

### Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

### Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

# Api service docker image over HTTPS

Generate certificate and configure local machine:

`dotnet dev-certs https --clean`

`dotnet dev-certs https -ep ${HOME}/.aspnet/dev-certs/trust/aspnetapp.pfx  -p [password]`

`dotnet dev-certs https --trust`

Update dockerfile:

`ENV ASPNETCORE_Kestrel__Certificates__Default__Password: "[password]"`

`ENV ASPNETCORE_Kestrel__Certificates__Default__Path: "/https/aspnetapp.pfx"`

