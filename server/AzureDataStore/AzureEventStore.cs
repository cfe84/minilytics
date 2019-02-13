using Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace AzureDataStore
{
    public class AzureEventStore : IEventStore
    {
        CloudTableClient client;
        CloudTable table;

        public AzureEventStore(string connectionString, string tableName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            client = account.CreateCloudTableClient();
            table = client.GetTableReference(tableName);
            table.CreateIfNotExistsAsync().Wait();
        }

        public async Task StoreEventAsync(Event evt)
        {
            var entity = new TableStorageEventEntity(evt);
            var operation = TableOperation.InsertOrReplace(entity);
            await table.ExecuteAsync(operation);
        }
    }
}
