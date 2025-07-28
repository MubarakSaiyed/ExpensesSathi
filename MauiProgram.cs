using Microsoft.Extensions.Logging;
using ExpensesSathi.Services; // Ensure to import the DebtService and TransactionService namespaces
using MudBlazor.Services;

namespace ExpensesSathi;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // Configure the app to use the main App class
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                // Register fonts
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add Blazor WebView for the MAUI app
        builder.Services.AddMauiBlazorWebView();

        // Register services for Dependency Injection (DI)
        builder.Services.AddSingleton<TransactionService>(); // Register TransactionService as singleton
        builder.Services.AddSingleton<DebtService>(); // Register DebtService as singleton
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddMudServices();


       


#if DEBUG
        // Enable developer tools for Blazor in debug mode
        builder.Services.AddBlazorWebViewDeveloperTools();

        // Add debug logging for the app in development
        builder.Logging.AddDebug();
#endif

        // Return the built MauiApp instance
        return builder.Build();
    }
}