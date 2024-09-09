using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;
using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.CleanArch1.Services.DatasetService.Abstraction;

public interface IDatasetService
{
    Task<ServiceResponse<GetDitailedDatasetDto>> AddDataset(AddDatasetDto addDatasetDto);
    Task<ServiceResponse<IEnumerable<GetDatasetPreviewDto>>> GetAllDatasets();
    Task<ServiceResponse<GetDitailedDatasetDto>> DeleteDataset(long datasetId);
    Task<ServiceResponse<GetDitailedDatasetDto>> GetSingleDataset(long datasetId);
    Task<ServiceResponse<GetGraphDto>> GetDataModel(long datasetId, string vertexIdentifier, string sourceIdentifier, string targetIdentifier);
    Task<ServiceResponse<GetGraphDto>> GetFilteredDataModel(GetSubGraphDto getSubGraphDto);
    Task<ServiceResponse<DetailDto>> GetSingleVertex(string objId);
    Task<ServiceResponse<DetailDto>> GetSingleEdge(string objId);
}
