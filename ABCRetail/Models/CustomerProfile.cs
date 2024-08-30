using Azure;
using Azure.Data.Tables;
using System;

namespace ABCRetail.Models
{
    public class CustomerProfile : ITableEntity
    {
        public string PartitionKey { get; set; } // e.g., "Customers"
        public string RowKey { get; set; } // e.g., Customer ID
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public CustomerProfile()
        {
            PartitionKey = "CustomerProfile";
            RowKey = Guid.NewGuid().ToString();
        }
    }

  }

