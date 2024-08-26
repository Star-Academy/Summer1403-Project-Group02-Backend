using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.Services.DataAdminService;

public interface IDataAdminService
{
    Task<ServiceResponse<string>> StoreData(string? edgeFile, string? vertexFile, string graphName
        , string? edgeEntityName, string vertexEntityName, string userName);

    ServiceResponse<List<GetDataGroupDto>> DisplayDataSet(string username);

    Task<ServiceResponse<DisplayGraphDto>> DisplayGeraphData(string databaseName, string sourceEdgeIdentifierFieldName,
        string destinationEdgeIdentifierFieldName, string vertexIdentifierFieldName);
}