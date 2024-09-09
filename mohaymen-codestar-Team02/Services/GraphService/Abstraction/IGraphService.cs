using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services.GraphService.Abstraction;

public interface IGraphService
{
    public (List<Vertex> vertices, List<Edge> edges) GetGraph(Dictionary<string, Dictionary<string, string>> vertices,
        Dictionary<string, Dictionary<string, string>> edges, string vertexIdentifierFieldName,
        string SourceIdentifierFieldName, string TargetIdentifierFieldName);
}