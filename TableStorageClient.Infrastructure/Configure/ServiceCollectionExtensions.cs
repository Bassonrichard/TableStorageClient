using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TableStorageClient.Infrastructure.Services;

namespace TableStorageClient.Infrastructure.Configure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTableStorageClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddTableServiceClient(configuration["table-storage:table"], preferMsi: true);
            });

            services.AddSingleton<IMyTableService, MyTableService>();
            return services;
        }
    }
}
