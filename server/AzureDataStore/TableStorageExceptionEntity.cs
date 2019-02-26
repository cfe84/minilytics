using Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureDataStore
{
    class TableStorageExceptionEntity : ITableEntity
    {
        ClientException exception;
        string etag;

        public TableStorageExceptionEntity(ClientException evt)
        {
            this.exception = evt;
        }

        public string PartitionKey
        {
            get => exception.ReceivedDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK");
            set => throw new NotImplementedException();
        }
        public string RowKey
        {
            get => exception.ExceptionId;
            set => throw new NotImplementedException();
        }
        public DateTimeOffset Timestamp
        {
            get => exception.ReceivedDateTime;
            set => throw new NotImplementedException();
        }

        public string ETag
        {
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
            var properties = typeof(ClientException).GetProperties();
            var dictionary = properties.ToDictionary(prop => prop.Name, prop => toEntityProperty(prop.GetValue(exception)));
            return dictionary;
        }
    }
}
