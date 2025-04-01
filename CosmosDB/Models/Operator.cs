

namespace CosmosGremlinDemo.Models
{
    public class Operator
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Label { get; set; } = "Operator";
        public string Pk { get; set; } = "opPartition";
        public string Symbol { get; set; }
    }
}
