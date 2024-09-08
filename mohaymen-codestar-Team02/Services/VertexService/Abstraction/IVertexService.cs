using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;

namespace mohaymen_codestar_Team02.Services;

public interface IVertexService
{
    public Task<Dictionary<string, Dictionary<string, string>>> FilterVertices(long dataSetId, Dictionary<string, string> vertexAttributeVales);

    DetailDto GetVertexDetails(string objId);
}