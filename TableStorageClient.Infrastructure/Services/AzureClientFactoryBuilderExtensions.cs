using Azure.Core.Extensions;
using Azure.Data.Tables;
using Microsoft.Extensions.Azure;

internal static class AzureClientFactoryBuilderExtensions
{
    public static IAzureClientBuilder<TableServiceClient, TableClientOptions> AddTableServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
    {
        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
        {
            return builder.AddTableServiceClient(serviceUri);
        }
        else
        {
            return builder.AddTableServiceClient(serviceUriOrConnectionString);
        }
    }
}
