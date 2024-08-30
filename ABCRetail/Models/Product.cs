using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; } // e.g., "Products"
        public string RowKey { get; set; } // e.g., Product ID
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public Product()
        {
            PartitionKey = "Product";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
