using Azure.Data.Tables;
using BenchmarkDotNet.Attributes;
using System.Text.Json;
using TableStorageClient.Infrastructure.Models;
using TableStorageClient.Infrastructure.Services;

namespace TableStorageClient.Console
{
    public class MyTableServiceBenchmark
    {
        private readonly IMyTableService _myTableService;
        private const string TableName = "Entities";

        public MyTableServiceBenchmark()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var tableServiceClient = new TableServiceClient(connectionString);
            _myTableService = new MyTableService(tableServiceClient);
        }

        [Benchmark]
        public async Task AddEntityBenchmark()
        {
            var myEntity = CreateNewEntity();
            await _myTableService.AddEntityAsync(TableName, myEntity);
            await _myTableService.DeleteEntityAsync(TableName, myEntity.PartitionKey, myEntity.RowKey);
        }

        [Benchmark]
        public async Task GetEntityBenchmark()
        {
            var myEntity = CreateNewEntity();
            await _myTableService.AddEntityAsync(TableName, myEntity);
            await _myTableService.GetEntityAsync<MyEntity>(TableName, myEntity.PartitionKey, myEntity.RowKey);
            await _myTableService.DeleteEntityAsync(TableName, myEntity.PartitionKey, myEntity.RowKey);
        }

        [Benchmark]
        public async Task UpdateEntityBenchmark()
        {
            var myEntity = CreateNewEntity();
            await _myTableService.AddEntityAsync(TableName, myEntity);
            await _myTableService.UpdateEntityAsync(TableName, myEntity);
            await _myTableService.DeleteEntityAsync(TableName, myEntity.PartitionKey, myEntity.RowKey);
        }

        private MyEntity CreateNewEntity()
        {
            return new MyEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Data = JsonSerializer.Serialize(new TableEntityData
                {
                    Data = "MY DATA",
                    child = new Child
                    {
                        childdata = "MY CHILD DATA"
                    }
                })
            };
        }
    }
}
