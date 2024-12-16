namespace FinancialNewsMonitor.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication RegisterComponents(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.RegisterServices();

        return webApplicationBuilder
            .Build()
            .RegisterWebApplicationComponents();
    }
}
