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
        CloudTable eventsTable, exceptionTable;

        public AzureEventStore(string connectionString, string eventsTableName, string exceptionTableName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            client = account.CreateCloudTableClient();
            eventsTable = client.GetTableReference(eventsTableName);
            eventsTable.CreateIfNotExistsAsync().Wait();
            exceptionTable = client.GetTableReference(exceptionTableName);
            exceptionTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task StoreEventAsync(Event evt)
        {
            var entity = new TableStorageEventEntity(evt);
            var operation = TableOperation.InsertOrReplace(entity);
            await eventsTable.ExecuteAsync(operation);
        }

        public async Task StoreExceptionAsync(ClientException exc)
        {
            var entity = new TableStorageExceptionEntity(exc);
            var operation = TableOperation.InsertOrReplace(entity);
            await exceptionTable.ExecuteAsync(operation);
        }
    }
}
