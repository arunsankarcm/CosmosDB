using CosmosGremlinDemo.Models;
using CosmosGremlinDemo.Services;
using static CosmosGremlinDemo.Services.GraphService;

var client = GremlinClientFactory.Create();

// 1) Create a Scenario
var scenario = new Scenario
{
    Pk = "scenarioPartition1",
    Name = "Main Scenario"
};
await GraphServiceNoCalc.AddScenarioVertex(client, scenario);

// 2) Create a UnitProcess
var process = new UnitProcess
{
    Pk = "processPartition1",
    Name = "Process1"
};
await GraphServiceNoCalc.AddUnitProcessVertex(client, process);

// Link Scenario -> UnitProcess
await GraphServiceNoCalc.LinkScenarioToUnitProcess(client, scenario.Id, scenario.Pk, process.Id, process.Pk);

// 3) Expression #1: c1 = v1 + v2
var v1 = new Variable { Pk = "varPartition1", Name = "v1" };
var v2 = new Variable { Pk = "varPartition1", Name = "v2" };
var c1 = new Variable { Pk = "varPartition1", Name = "c1" };

await GraphServiceNoCalc.AddVariableVertex(client, v1);
await GraphServiceNoCalc.AddVariableVertex(client, v2);
await GraphServiceNoCalc.AddVariableVertex(client, c1);

var plusOpForC1 = new Operator
{
    Pk = "opPartition1",
    Symbol = "+"
};
await GraphServiceNoCalc.AddOperatorVertex(client, plusOpForC1);

await GraphServiceNoCalc.LinkUnitProcessToOperator(client, process.Id, process.Pk, plusOpForC1.Id, plusOpForC1.Pk);

await GraphServiceNoCalc.AddOperandEdge(client, plusOpForC1.Id, plusOpForC1.Pk, v1.Id, v1.Pk);
await GraphServiceNoCalc.AddOperandEdge(client, plusOpForC1.Id, plusOpForC1.Pk, v2.Id, v2.Pk);
await GraphServiceNoCalc.AddProducesEdge(client, plusOpForC1.Id, plusOpForC1.Pk, c1.Id, c1.Pk);

// Direct dependency edges for c1
await GraphServiceNoCalc.AddDependsOnEdge(client, c1.Id, c1.Pk, v1.Id, v1.Pk, "+");
await GraphServiceNoCalc.AddDependsOnEdge(client, c1.Id, c1.Pk, v2.Id, v2.Pk, "+");

Console.WriteLine("Inserted c1 = v1 + v2");

// 4) Expression #2: c3 = (c1 + v3) * v4
var v3 = new Variable { Pk = "varPartition1", Name = "v3" };
var v4 = new Variable { Pk = "varPartition1", Name = "v4" };
var c3 = new Variable { Pk = "varPartition1", Name = "c3" };

await GraphServiceNoCalc.AddVariableVertex(client, v3);
await GraphServiceNoCalc.AddVariableVertex(client, v4);
await GraphServiceNoCalc.AddVariableVertex(client, c3);

var multiplyOp = new Operator
{
    Pk = "opPartition1",
    Symbol = "*"
};
await GraphServiceNoCalc.AddOperatorVertex(client, multiplyOp);

var plusOpForC3 = new Operator
{
    Pk = "opPartition1",
    Symbol = "+"
};
await GraphServiceNoCalc.AddOperatorVertex(client, plusOpForC3);

await GraphServiceNoCalc.LinkUnitProcessToOperator(client, process.Id, process.Pk, multiplyOp.Id, multiplyOp.Pk);

await GraphServiceNoCalc.AddOperandEdge(client, multiplyOp.Id, multiplyOp.Pk, plusOpForC3.Id, plusOpForC3.Pk);
await GraphServiceNoCalc.AddOperandEdge(client, multiplyOp.Id, multiplyOp.Pk, v4.Id, v4.Pk);
await GraphServiceNoCalc.AddOperandEdge(client, plusOpForC3.Id, plusOpForC3.Pk, c1.Id, c1.Pk);
await GraphServiceNoCalc.AddOperandEdge(client, plusOpForC3.Id, plusOpForC3.Pk, v3.Id, v3.Pk);
await GraphServiceNoCalc.AddProducesEdge(client, multiplyOp.Id, multiplyOp.Pk, c3.Id, c3.Pk);

// Direct dependency edges for c3
await GraphServiceNoCalc.AddDependsOnEdge(client, c3.Id, c3.Pk, c1.Id, c1.Pk, "*");
await GraphServiceNoCalc.AddDependsOnEdge(client, c3.Id, c3.Pk, v3.Id, v3.Pk, "*");
await GraphServiceNoCalc.AddDependsOnEdge(client, c3.Id, c3.Pk, v4.Id, v4.Pk, "*");

Console.WriteLine("Inserted c3 = (c1 + v3) * v4");

Console.WriteLine("All done! Press any key to exit.");
Console.ReadKey();
