using Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDataStore
{
    class TableStorageEventEntity: ITableEntity
    {
        Event evt;
        string etag;

        public TableStorageEventEntity(Event evt)
        {
            this.evt = evt;
        }

        public string PartitionKey {
            get => evt.ReceivedDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK");
            set => throw new NotImplementedException();
        }
        public string RowKey {
            get => evt.EventId;
            set => throw new NotImplementedException();
        }
        public DateTimeOffset Timestamp {
            get => evt.ReceivedDateTime;
            set => throw new NotImplementedException();
        }

        public string ETag {
            get => etag;
            set => etag = value;
        }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        EntityProperty toEntityProperty(object value)
        {
            if (value == null)
                return new EntityProperty("");
            var type = value.GetType();
            if (type == typeof(string))
                return new EntityProperty((string)value);
            if (type == typeof(int))
                return new EntityProperty((int)value);
            if (type == typeof(DateTime))
                return new EntityProperty((DateTime)value);
            if (type == typeof(bool))
                return new EntityProperty((bool)value);
            throw new InvalidCastException("Unknown type: " + type.Name);
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = typeof(Event).GetProperties();
            var dictionary = properties.ToDictionary(prop => prop.Name, prop => toEntityProperty(prop.GetValue(evt)));
            return dictionary;
        }
    }
}
