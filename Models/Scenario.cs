

namespace CosmosGremlinDemo.Models
{
    public class Scenario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Label { get; set; } = "Scenario";
        public string Pk { get; set; } = "scenarioPartition";
        public string Name { get; set; }
    }
}
