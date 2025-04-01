using CosmosGremlinDemo.Models;
using Gremlin.Net.Driver;

namespace CosmosGremlinDemo.Services
{
    public static class GraphService
    {
        // -------------------------------------
        // Vertex creation
        // -------------------------------------
        public static async Task AddScenarioVertex(GremlinClient client, Scenario scenario)
        {
            var query = $"g.addV('{scenario.Label}')"
                      + $".property('id', '{scenario.Id}')"
                      + $".property('pk', '{scenario.Pk}')"
                      + $".property('name', '{scenario.Name}')";
            await client.SubmitAsync<dynamic>(query);
        }

        public static async Task AddUnitProcessVertex(GremlinClient client, UnitProcess process)
        {
            var query = $"g.addV('{process.Label}')"
                      + $".property('id', '{process.Id}')"
                      + $".property('pk', '{process.Pk}')"
                      + $".property('name', '{process.Name}')";
            await client.SubmitAsync<dynamic>(query);
        }

        public static async Task AddCalculationVertex(GremlinClient client, Calculation calc)
        {
            var query = $"g.addV('{calc.Label}')"
                      + $".property('id', '{calc.Id}')"
                      + $".property('pk', '{calc.Pk}')";

            if (!string.IsNullOrEmpty(calc.Expression))
            {
                query += $".property('expression', '{calc.Expression}')";
            }
            if (!string.IsNullOrEmpty(calc.Operator))
            {
                query += $".property('operator', '{calc.Operator}')";
            }

            await client.SubmitAsync<dynamic>(query);
        }

        public static async Task AddVariableVertex(GremlinClient client, Variable variable)
        {
            var query = $"g.addV('{variable.Label}')"
                      + $".property('id', '{variable.Id}')"
                      + $".property('pk', '{variable.Pk}')"
                      + $".property('name', '{variable.Name}')";
            await client.SubmitAsync<dynamic>(query);
        }

        // If you want an Operator vertex:
        public static async Task AddOperatorVertex(GremlinClient client, Operator op)
        {
            var query = $"g.addV('{op.Label}')"
                      + $".property('id', '{op.Id}')"
                      + $".property('pk', '{op.Pk}')"
                      + $".property('symbol', '{op.Symbol}')";
            await client.SubmitAsync<dynamic>(query);
        }

        // -------------------------------------
        // Edge creation
        // -------------------------------------

        // Generic helper to create an edge with any label
        public static async Task AddEdge(GremlinClient client,
            string fromId, string fromPk,
            string toId, string toPk,
            string edgeLabel)
        {
            var query =
                $"g.V('{fromId}').has('pk','{fromPk}')"
              + $".addE('{edgeLabel}')"
              + $".to(g.V('{toId}').has('pk','{toPk}'))";

            await client.SubmitAsync<dynamic>(query);
        }

        // Typical edges for the hierarchical relationships:
        public static async Task LinkTaskToUnitProcess(GremlinClient client,
            string scenarioId, string scenarioPk,
            string processId, string processPk)
        {
            await AddEdge(client, scenarioId, scenarioPk, processId, processPk, "HAS_UNIT_PROCESS");
        }

        public static async Task LinkUnitProcessToCalculation(GremlinClient client,
            string processId, string processPk,
            string calcId, string calcPk)
        {
            await AddEdge(client, processId, processPk, calcId, calcPk, "HAS_CALCULATION");
        }

        // Typical edges for dependency representation:
        public static async Task AddConsumesEdge(GremlinClient client,
            string calcId, string calcPk,
            string varId, string varPk)
        {
            await AddEdge(client, calcId, calcPk, varId, varPk, "CONSUMES");
        }

        public static async Task AddProducesEdge(GremlinClient client,
            string calcId, string calcPk,
            string varId, string varPk)
        {
            await AddEdge(client, calcId, calcPk, varId, varPk, "PRODUCES");
        }

        // For Operator-based parse trees (optional):
        public static async Task LinkCalculationToOperator(GremlinClient client,
            string calcId, string calcPk,
            string opId, string opPk)
        {
            await AddEdge(client, calcId, calcPk, opId, opPk, "USES_OPERATOR");
        }

        public static async Task AddOperandEdge(GremlinClient client,
            string opId, string opPk,
            string varOrOpId, string varOrOpPk)
        {
            await AddEdge(client, opId, opPk, varOrOpId, varOrOpPk, "OPERAND");
        }
    }
}
