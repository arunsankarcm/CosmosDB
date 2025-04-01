
namespace CosmosGremlinDemo.Models
{
    public class Variable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Label { get; set; } = "Variable";
        public string Pk { get; set; } = "varPartition";
        public string Name { get; set; }   // E.g. "v1", "c1"
        //public double Value { get; set; }
    }
}
