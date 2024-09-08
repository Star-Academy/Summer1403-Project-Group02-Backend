using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services;

public interface IDisplayDataService
{
    (List<Vertex> vertices, List<Edge> edges) GetGraph(string databaseName,
        string sourceEdgeIdentifierFieldName,
        string destinationEdgeIdentifierFieldName, string vertexIdentifierFieldName);
}