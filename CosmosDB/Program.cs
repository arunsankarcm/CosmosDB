using CosmosGremlinDemo.Models;
using CosmosGremlinDemo.Services;

var client = GremlinClientFactory.Create();

// 1. Create a Scenario
var scenario = new Scenario
{
    Pk = "scenarioPartition1",
    Name = "Main Task"
};
await GraphService.AddScenarioVertex(client, scenario);

// 2. Create a Unit Process under that Scenario
var process = new UnitProcess
{
    Pk = "processPartition1",
    Name = "Process1"
};
await GraphService.AddUnitProcessVertex(client, process);

// Link Scenario -> Unit Process
await GraphService.LinkTaskToUnitProcess(client, scenario.Id, scenario.Pk, process.Id, process.Pk);

// 3. Simple Calculation: "c1 = v1 + v2" (with Operator vertex)
var calc1 = new Calculation
{
    Pk = "calcPartition1",
    Expression = "c1 = v1 + v2"
};
await GraphService.AddCalculationVertex(client, calc1);
await GraphService.LinkUnitProcessToCalculation(client, process.Id, process.Pk, calc1.Id, calc1.Pk);

var v1 = new Variable { Pk = "varPartition1", Name = "v1" };
var v2 = new Variable { Pk = "varPartition1", Name = "v2" };
var c1 = new Variable { Pk = "varPartition1", Name = "c1" };

await GraphService.AddVariableVertex(client, v1);
await GraphService.AddVariableVertex(client, v2);
await GraphService.AddVariableVertex(client, c1);

// Create Operator for simple calculation
var opPlusSimple = new Operator { Pk = "opPartition1", Symbol = "+" };
await GraphService.AddOperatorVertex(client, opPlusSimple);

// Link Calculation to Operator
await GraphService.LinkCalculationToOperator(client, calc1.Id, calc1.Pk, opPlusSimple.Id, opPlusSimple.Pk);

// Operator connects to operands v1 and v2
await GraphService.AddOperandEdge(client, opPlusSimple.Id, opPlusSimple.Pk, v1.Id, v1.Pk);
await GraphService.AddOperandEdge(client, opPlusSimple.Id, opPlusSimple.Pk, v2.Id, v2.Pk);

// Calculation produces c1
await GraphService.AddProducesEdge(client, calc1.Id, calc1.Pk, c1.Id, c1.Pk);

// 4. Complex Calculation: "c3 = (c1 + v3) * v4" using Operator vertices
var calc3 = new Calculation
{
    Pk = "calcPartition2",
    Expression = "c3 = (c1 + v3) * v4"
};
await GraphService.AddCalculationVertex(client, calc3);
await GraphService.LinkUnitProcessToCalculation(client, process.Id, process.Pk, calc3.Id, calc3.Pk);

var v3 = new Variable { Pk = "varPartition1", Name = "v3" };
var v4 = new Variable { Pk = "varPartition1", Name = "v4" };
var c3 = new Variable { Pk = "varPartition1", Name = "c3" };

await GraphService.AddVariableVertex(client, v3);
await GraphService.AddVariableVertex(client, v4);
await GraphService.AddVariableVertex(client, c3);

// Create Operators explicitly
var opMultiply = new Operator { Pk = "opPartition1", Symbol = "*" };
var opPlus = new Operator { Pk = "opPartition1", Symbol = "+" };

await GraphService.AddOperatorVertex(client, opMultiply);
await GraphService.AddOperatorVertex(client, opPlus);

// Link Calculation to Operators
await GraphService.LinkCalculationToOperator(client, calc3.Id, calc3.Pk, opMultiply.Id, opMultiply.Pk);

// Build operator tree: (*) operator connects to (+) operator and v4
await GraphService.AddOperandEdge(client, opMultiply.Id, opMultiply.Pk, opPlus.Id, opPlus.Pk);
await GraphService.AddOperandEdge(client, opMultiply.Id, opMultiply.Pk, v4.Id, v4.Pk);

// (+) operator connects to c1 and v3
await GraphService.AddOperandEdge(client, opPlus.Id, opPlus.Pk, c1.Id, c1.Pk);
await GraphService.AddOperandEdge(client, opPlus.Id, opPlus.Pk, v3.Id, v3.Pk);

// Calculation produces c3
await GraphService.AddProducesEdge(client, calc3.Id, calc3.Pk, c3.Id, c3.Pk);

Console.WriteLine("Complex calculation with operators inserted successfully!");
