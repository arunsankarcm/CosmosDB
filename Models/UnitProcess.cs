
namespace CosmosGremlinDemo.Models
{
    public class UnitProcess
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Label { get; set; } = "UnitProcess";
        public string Pk { get; set; } = "processPartition";
        public string Name { get; set; }
    }
}
