# LunchSplit

LunchSplit is a .NET 8 console app that splits a restaurant bill among attendees.


## Features

- Calculates no tip, a percentage tip, or a fixed tip.

- Splits the total equally or by attendee weight.

- Gives excluded attendees a zero share.

- Rounds shares and reconciles cents so the shares add up to the total.

- Prints an itemized receipt.


## Requirements

.NET 10 SDK, .NET 8 runtime and either Visual Studio or the .NET command-line tools.


## Build and run

Run these commands from the repository folder:

```powershell

dotnet build LunchSplit/LunchSplit.csproj

dotnet run --project LunchSplit/LunchSplit.csproj

```

`Program.cs` runs a sample bill and prints its receipt.

## Square Sandbox Setup

The payment integration tests use Square Sandbox. Copy appsettings.example.json to appsettings.json in the repository root and in the Square section, set AccessToken and LocationId to the Sandbox access token and CAD location ID from a Canadian test account. [Square Sandbox setup](https://developer.squareup.com/docs/devtools/sandbox/overview)


## Tests

The test project is `LunchSplit.Tests`. A full test run shows 41 passing tests.

Run the local tests without Square credentials:

```powershell
dotnet test --filter "TestCategory!=Tier1"
```

After setting up Square Sandbox, run the full suite from the repository root:

```powershell
dotnet test
```

See [CODEMAP.md](CODEMAP.md) for the source and test locations.
