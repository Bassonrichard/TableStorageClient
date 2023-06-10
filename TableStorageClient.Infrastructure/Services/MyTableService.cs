using Azure.Data.Tables;
using Azure;
using System.Collections.Concurrent;
using TableStorageClient.Infrastructure.Models;

namespace TableStorageClient.Infrastructure.Services
{
    public interface IMyTableService
    {
        Task AddEntityAsync<TEntity>(string tableName, TEntity entity) where TEntity : class, ITableEntity;
        Task<TEntity> GetEntityAsync<TEntity>(string tableName, string partitionKey, string rowKey) where TEntity : class, ITableEntity;
        Task UpdateEntityAsync(string tableName, MyEntity entity);
        Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey);
    }

    public class MyTableService : IMyTableService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ConcurrentDictionary<string, TableClient> _tableClients;

        public MyTableService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
            _tableClients = new ConcurrentDictionary<string, TableClient>();
        }

        public async Task AddEntityAsync<TEntity>(string tableName, TEntity entity) where TEntity : class, ITableEntity
        {
            var tableClient = await GetTableClientAsync(tableName);
            await tableClient.AddEntityAsync(entity);
        }

        public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey)
        {
            var tableClient = await GetTableClientAsync(tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey, ETag.All);
        }

        public async Task<TEntity> GetEntityAsync<TEntity>(string tableName, string partitionKey, string rowKey) where TEntity : class, ITableEntity
        {
            var tableClient = await GetTableClientAsync(tableName);
            var response = await tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task UpdateEntityAsync(string tableName, MyEntity entity)
        {
            var tableClient = await GetTableClientAsync(tableName);
            await tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        private async Task<TableClient> GetTableClientAsync(string tableName)
        {
            // Check if table client already exists
            if (!_tableClients.TryGetValue(tableName, out var tableClient))
            {
                // If not, create it and add to the dictionary
                await _tableServiceClient.CreateTableIfNotExistsAsync(tableName);
                tableClient = _tableServiceClient.GetTableClient(tableName);
                _tableClients[tableName] = tableClient;
            }
            return tableClient;
        }
    }
}
