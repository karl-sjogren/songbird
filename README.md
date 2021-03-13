# Songbird

Small intranet stand-in to use at work until we get around to developing something for real.

## Requirements

The following things needs to be installed on the development machine.

* Node JS >= 12 (https://nodejs.org/)
    * Ember CLI (https://www.npmjs.com/package/ember-cli)
* Yarn > 1.22 (https://yarnpkg.com/)
* .NET 5.0 (https://dot.net)
* GIT (https://git-scm.com/)

## Running the project

To run this project two terminal windows is needed (or preferably a split terminal window).

### Window 1

Window 1 should be started as Administrator (assuming this is run on Windows since Ember
wants to use symlinks. This speeds up the build process **a lot**.

```
cd src/Songbird.Frontend/
ember s
```

### Window 2

```
cd src/Songbird.Web/
dotnet watch run
```

The project is now available at https://localhost:5001/.

## Entity Framework Migrations

```
dotnet ef migrations add AddUser // Add a new migration named AddUser
dotnet ef migrations remove // Remove the latest migration
dotnet ef database update // Apply all migrations
dotnet ef database update AddUser // Apply all migrations up to, or remove down to, the AddUser migration.
```
