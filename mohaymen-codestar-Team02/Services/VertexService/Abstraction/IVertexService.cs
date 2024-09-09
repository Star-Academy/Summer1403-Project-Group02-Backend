using mohaymen_codestar_Team02.Dtos.Extra.GraphDto;

namespace mohaymen_codestar_Team02.Services.VertexService.Abstraction;

public interface IVertexService
{
    Task<Dictionary<string, Dictionary<string, string>>> FilterVertices(long dataSetId, Dictionary<string, string> vertexAttributeVales);
    DetailDto GetVertexDetails(string objId);
}