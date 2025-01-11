The following commands must be run from the root of the project.

### Add a migration
```
dotnet ef migrations add <Name> --project Spot.Data --startup-project Spot.Server --context ApplicationContext
```

### Remove a migration
```
dotnet ef migrations remove --project Spot.Data --startup-project Spot.Server --context ApplicationContext
```

### Update the database
```
dotnet ef database update --project Spot.Data --startup-project Spot.Server --context ApplicationContext
```

### Revert migration
```
dotnet ef database update <Name> --project Spot.Data --startup-project Spot.Server --context ApplicationContext
```
