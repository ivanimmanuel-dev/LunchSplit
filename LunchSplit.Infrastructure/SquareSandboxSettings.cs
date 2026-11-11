using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LunchSplit.Infrastructure;

/// <summary>Loads Sandbox credentials from a local settings file.</summary>
public record SquareSandboxSettings(string AccessToken, string LocationId)
{
    /// <summary>Reads the Square section of the settings file.</summary>
    public static SquareSandboxSettings Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new InvalidOperationException("Create appsettings.json from appsettings.example.json and enter your Square Sandbox credentials.");
        }

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var square = document.RootElement.GetProperty("Square");

        string token = square.GetProperty("AccessToken").GetString() ?? "";
        string location = square.GetProperty("LocationId").GetString() ?? "";

        if (string.IsNullOrWhiteSpace(token) || token.StartsWith("YOUR_") || string.IsNullOrWhiteSpace(location) || location.StartsWith("YOUR_"))
        {
            throw new InvalidOperationException("Set valid Sandbox credentials in appsettings.json.");
        }

        return new SquareSandboxSettings(token, location);
    }
}
