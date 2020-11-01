using System;
using Microsoft.WindowsAzure.Storage.Table;
namespace marcovicit5
{
    public class MetricsEntity : TableEntity
    {
        public MetricsEntity(string university)
        {
            DateTime now = DateTime.UtcNow;
            this.PartitionKey = university;
            this.RowKey = now.ToString().Replace("/", ".");
        }

        public MetricsEntity()
        {

        }
        public int Count { get; set; }
    }
} 