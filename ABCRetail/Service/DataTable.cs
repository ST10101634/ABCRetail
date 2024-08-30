using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABCRetail.Service
{
    public class DataTable
    {
        private readonly TableServiceClient _tableServiceClient;

        public DataTable(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public TableClient GetTableClient(string tableName)
        {
            return _tableServiceClient.GetTableClient(tableName);
        }

        public async Task InsertOrMergeEntityAsync<T>(TableClient tableClient, T entity) where T : class, ITableEntity, new()
        {
            await tableClient.CreateIfNotExistsAsync();
            await tableClient.UpsertEntityAsync(entity);
        }

        public async Task<T> RetrieveEntityAsync<T>(TableClient tableClient, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            var response = await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task DeleteEntityAsync(TableClient tableClient, string partitionKey, string rowKey)
        {
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<T>> ListEntitiesAsync<T>(TableClient tableClient) where T : class, ITableEntity, new()
        {
            var entities = new List<T>();
            await foreach (var entity in tableClient.QueryAsync<T>())
            {
                entities.Add(entity);
            }
            return entities;
        }
    }
}
