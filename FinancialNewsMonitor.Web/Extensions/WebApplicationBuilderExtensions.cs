using FinancialNewsMonitor.TypeHandlers;

namespace FinancialNewsMonitor.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication RegisterComponents(this WebApplicationBuilder webApplicationBuilder)
        => webApplicationBuilder
            .RegisterServices()
            .Build()
            .RegisterWebApplicationComponents();

    private static WebApplicationBuilder RegisterServices(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.RegisterServices();
        AddTypeHandlers(webApplicationBuilder);

        return webApplicationBuilder;
    }

    private static WebApplicationBuilder AddTypeHandlers(this WebApplicationBuilder webApplicationBuilder)
    {
        Dapper.SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        return webApplicationBuilder;
    }
}
