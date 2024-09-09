using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;
using QuikGraph;

namespace mohaymen_codestar_Team02.Services;

public interface IEdgeService
{
    Task<Dictionary<string, Dictionary<string, string>>> FilterEdges(long dataSetId, Dictionary<string, string> edgeAttributeVales);
    DetailDto GetEdgeDetails(string objId);
}