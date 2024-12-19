using FinancialNewsMonitor.Cqrs.Queries;
using FinancialNewsMonitor.DataAccessLayer.Resolvers;
using FinancialNewsMonitor.Mappers;
using FinancialNewsMonitor.Services;

namespace FinancialNewsMonitor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddCors(options => 
            options.AddDefaultPolicy(
                builder => 
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddLogging();

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetExternalSymbolSearchQuery).Assembly));

        var configurationSection = services
            .BuildServiceProvider()
            .GetRequiredService<IConfiguration>()
            .GetRequiredSection("ExternalApiUrl") ?? throw new ArgumentNullException("ExternalApiUrl");

        services.AddHttpClient<IAlphaVantageService, AlphaVantageService>(client =>
        {
            client.BaseAddress = new Uri(
                configurationSection.GetValue<string>("AlphaVantageApiUrl") ?? throw new ArgumentNullException("AlphaVantageApiUrl"));
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddDefaultLogger();

        services.AddScoped<IStockDataModelMapper, StockDataMapper>();
        services.AddScoped<ISymbolStockDataMapper, SymbolStockDataMapper>();
        services.AddScoped<ISymbolMapper, SymbolMapper>();
        services.AddScoped<IAlphaVantageApiResultMapper, AlphaVantageApiResultMapper>();
        services.AddScoped<IUnitOfWorkSessionResolver, UnitOfWorkSessionResolver>();
        services.AddScoped<IFinancialService, FinancialService>();

        //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
        //        .AddNegotiate();

        //services.AddAuthorization(options =>
        //{
        //    options.FallbackPolicy = options.DefaultPolicy;
        //});

        return services;
    }
}
