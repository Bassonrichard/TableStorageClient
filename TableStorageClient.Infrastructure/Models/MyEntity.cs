using Azure;
using Azure.Data.Tables;
using System.Text.Json;

namespace TableStorageClient.Infrastructure.Models
{
    public class MyEntity : TableEntityData, ITableEntity
    {
        public MyEntity() { }

        public MyEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
        public ETag ETag { get; set; }
    }
}
