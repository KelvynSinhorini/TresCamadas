using TresCamadas.Business.Interfaces;
using TresCamadas.Business.Notificacoes;
using TresCamadas.Business.Services;
using TresCamadas.Data.Context;
using TresCamadas.Data.Repository;

namespace TresCamadas.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        // Data
        services.AddScoped<TresCamadasDbContext>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();

        // Business
        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<INotificador, Notificador>();

        return services;
    }
}
