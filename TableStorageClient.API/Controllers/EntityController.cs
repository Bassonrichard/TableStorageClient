using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TableStorageClient.Infrastructure.Models;
using TableStorageClient.Infrastructure.Services;

namespace TableStorageClient.API.Controllers
{
    [Route("api/Entity")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly ILogger<EntityController> _logger;
        private readonly IMyTableService _tableService;
        private const string TABLE_NAME = "Entity";

        public EntityController(ILogger<EntityController> logger, IMyTableService tableService)
        {
            _logger = logger;
            _tableService = tableService;
        }

        [HttpPost]
        [Route("Submit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableEntityData))]
        public async Task<IActionResult> Post([FromBody] TableEntityData data)
        {
            try
            {
                
                _logger.LogInformation("Received request to submit data {@data}", data);
                var entity = new MyEntity(data.Role, data.Id)
                {
                    Data = JsonSerializer.Serialize(data)
                };

                await _tableService.AddEntityAsync(TABLE_NAME, entity);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write entity to table {errorMessage}",  ex.Message);

                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableEntityData))]
        public async Task<IActionResult> Get([FromQuery] string partitionKey, [FromQuery] string rowKey)
        {
            try
            {
                 var entity = await _tableService.GetEntityAsync<MyEntity>(TABLE_NAME, partitionKey, rowKey);

                var data = JsonSerializer.Deserialize<TableEntityData>(entity.Data);
                _logger.LogInformation("Retrieved Entity {@data}", data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write entity to table {errorMessage}", ex.Message);

                throw;
            }
        }

        [HttpPatch]
        [Route("Patch")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableEntityData))]
        public async Task<IActionResult> Patch([FromQuery] string partitionKey, [FromQuery] string rowKey, [FromBody] TableEntityData data)
        {
            try
            {
                var entity = await _tableService.GetEntityAsync<MyEntity>(TABLE_NAME, partitionKey, rowKey);
                entity.Data = JsonSerializer.Serialize(data);

                await _tableService.UpdateEntityAsync(TABLE_NAME, entity);

                _logger.LogInformation("Updated Entity {@data}", data);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write entity to table {errorMessage}", ex.Message);

                throw;
            }
        }
    }
}
