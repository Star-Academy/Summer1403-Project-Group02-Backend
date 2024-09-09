using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services.GraphService;

public class GraphService : IGraphService
{
    public (List<Vertex> vertices, List<Edge> edges) GetGraph(Dictionary<string, Dictionary<string, string>> vertices,
        Dictionary<string, Dictionary<string, string>> edges, string vertexIdentifierFieldName,
        string sourceIdentifierFieldName, string targetIdentifierFieldName)
    {
        var resEdges = new List<Edge>();

        var dicVertices = vertices.GroupBy(x => x.Value[vertexIdentifierFieldName])
            .ToDictionary(x => x.Key, x => x.ToList());

        var resVertices = vertices
            .Select(record => new Vertex
            {
                Id = record.Key,
                Label = record.Value[vertexIdentifierFieldName]
            })
            .ToList();

        foreach (var edge in edges)
        {
            var sourceValue = edge.Value[sourceIdentifierFieldName];
            var targetValue = edge.Value[targetIdentifierFieldName];

            List<KeyValuePair<string, Dictionary<string, string>>> sources;
            if (!dicVertices.TryGetValue(sourceValue, out sources)) continue;
            List<KeyValuePair<string, Dictionary<string, string>>> targets;
            if (!dicVertices.TryGetValue(targetValue, out targets)) continue;

            foreach (var source in sources)
            foreach (var target in targets)
            {
                var newEdge = new Edge()
                {
                    Id = edge.Key,
                    Source = source.Key,
                    Target = target.Key
                };
                resEdges.Add(newEdge);
            }
        }

        return (resVertices, resEdges);
    }
}