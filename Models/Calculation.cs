

namespace CosmosGremlinDemo.Models
{
    public class Calculation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Label { get; set; } = "Calculation";
        public string Pk { get; set; } = "calcPartition";

        // If you want a textual expression for reference: "c2 = c1 + v3"
        public string Expression { get; set; }
        // If you only have a single operator, you could store it here (optional):
        public string Operator { get; set; }
    }
}
