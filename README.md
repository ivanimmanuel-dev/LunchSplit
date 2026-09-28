# LunchSplit

LunchSplit is a .NET 8 console app that splits a restaurant bill among attendees.


## Features

- Calculates no tip, a percentage tip, or a fixed tip.

- Splits the total equally or by attendee weight.

- Gives excluded attendees a zero share.

- Rounds shares and reconciles cents so the shares add up to the total.

- Prints an itemized receipt.


## Requirements

The .NET 8 SDK and Visual Studio or the .NET command-line tools.


## Build and run

Run these commands from the repository folder:

```powershell

dotnet build LunchSplit/LunchSplit.csproj

dotnet run --project LunchSplit/LunchSplit.csproj

```

`Program.cs` runs a sample bill and prints its receipt.


## Tests

The test project is `LunchSplit.Tests`. A full test run shows 31 passing tests and two expected failures:

```powershell

dotnet test LunchSplit.Tests/LunchSplit.Tests.csproj

```

To run the 31 implemented tests on their own:

```powershell

dotnet test LunchSplit.Tests/LunchSplit.Tests.csproj --filter "FullyQualifiedName!~CsvReceiptExporter&FullyQualifiedName!~PaymentRequest"

```

