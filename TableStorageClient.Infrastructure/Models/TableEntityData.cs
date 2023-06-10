namespace TableStorageClient.Infrastructure.Models
{
    public class TableEntityData
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string Role { get; init; } = "SUP";
        public string Data { get; set; }

        public Child child { get; set; }
    }
}
