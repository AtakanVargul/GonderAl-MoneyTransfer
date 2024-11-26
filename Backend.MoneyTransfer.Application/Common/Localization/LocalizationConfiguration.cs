using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.MoneyTransfer.Application.Common.Localization;

public static class LocalizationConfiguration
{
    public static void ConfigureLocalization(this IServiceCollection services)
    {
        services.AddLocalization(x => x.ResourcesPath = "Resources");
    }

    public static void ConfigureLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "en", "tr" };

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
    }
}