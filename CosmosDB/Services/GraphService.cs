using CosmosGremlinDemo.Models;
using Gremlin.Net.Driver;

namespace CosmosGremlinDemo.Services
{
    public static class GraphService
    {
        public static class GraphServiceNoCalc
        {
            // Vertex insertions
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

            public static async Task AddOperatorVertex(GremlinClient client, Operator op)
            {
                var query = $"g.addV('{op.Label}')"
                          + $".property('id', '{op.Id}')"
                          + $".property('pk', '{op.Pk}')"
                          + $".property('symbol', '{op.Symbol}')";
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

            // Edges
            private static async Task AddEdge(GremlinClient client,
                string fromId, string fromPk,
                string toId, string toPk,
                string edgeLabel, string operatorSymbol = null)
            {
                var query = $"g.V('{fromId}').has('pk','{fromPk}')"
                          + $".addE('{edgeLabel}')"
                          + (operatorSymbol != null ? $".property('operator','{operatorSymbol}')" : "")
                          + $".to(g.V('{toId}').has('pk','{toPk}'))";
                await client.SubmitAsync<dynamic>(query);
            }

            public static async Task LinkScenarioToUnitProcess(
                GremlinClient client,
                string scenarioId, string scenarioPk,
                string processId, string processPk)
            {
                await AddEdge(client, scenarioId, scenarioPk, processId, processPk, "HAS_UNIT_PROCESS");
            }

            public static async Task LinkUnitProcessToOperator(
                GremlinClient client,
                string processId, string processPk,
                string opId, string opPk)
            {
                await AddEdge(client, processId, processPk, opId, opPk, "HAS_OPERATOR");
            }

            public static async Task AddOperandEdge(
                GremlinClient client,
                string opId, string opPk,
                string varOrOpId, string varOrOpPk)
            {
                await AddEdge(client, opId, opPk, varOrOpId, varOrOpPk, "OPERAND");
            }

            public static async Task AddProducesEdge(
                GremlinClient client,
                string opId, string opPk,
                string varId, string varPk)
            {
                await AddEdge(client, opId, opPk, varId, varPk, "PRODUCES");
            }

            public static async Task AddDependsOnEdge(
                GremlinClient client,
                string outputVarId, string outputVarPk,
                string inputVarId, string inputVarPk,
                string operatorSymbol)
            {
                await AddEdge(client, outputVarId, outputVarPk, inputVarId, inputVarPk, "DEPENDS_ON", operatorSymbol);
            }
        }
    }
}
