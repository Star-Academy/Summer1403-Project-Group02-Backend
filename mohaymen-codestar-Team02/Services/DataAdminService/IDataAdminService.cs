using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Dto.InfoDto;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services.DataAdminService;

public interface IDataAdminService
{
    void StoreFile(FormFile file);
    (List<Vertex>, List<Edge>) GetGraph(string tableNameVertex, string tableNameEdge, string sourceAtt,
        string targetAtt, string vertexAtt);
    InfoDto GetEdgeData(string id);
    InfoDto GetVertexData(string id);

    Task<ServiceResponse<string>> StoreData(string? edgeFile, string? vertexFile, string graphName
        , string? edgeEntityName, string vertexEntityName, string userName);

    Task<ServiceResponse<string>> DisplayDataSet();

    Task<ServiceResponse<DisplayGraphDto>> DisplayGeraphData(string databaseName, string sourceEdgeIdentifierFieldName,
        string destinationEdgeIdentifierFieldName, string vertexIdentifierFieldName);
}