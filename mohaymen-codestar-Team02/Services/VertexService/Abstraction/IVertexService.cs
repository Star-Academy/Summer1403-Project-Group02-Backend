using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services;

public interface IVertexService
{
    Task<Dictionary<string, Dictionary<string, string>>> FilterVertices(long dataSetId, Dictionary<string, string> vertexAttributeVales);
    //Task<> AddVertex(long datasetId, string vertexEntityName,  IEnumerable<string> vertexAttributes, IEnumerable<IEnumerable<string>> vertexValues);
    DetailDto GetVertexDetails(string objId);
}