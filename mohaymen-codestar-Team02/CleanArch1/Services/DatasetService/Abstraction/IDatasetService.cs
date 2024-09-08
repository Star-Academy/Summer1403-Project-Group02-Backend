using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;
using WebApplication15.Dtos.Dataset;

namespace WebApplication15.Services;

public interface IDatasetService
{
    Task<ServiceResponse<GetDitailedDatasetDto>> AddDataset(AddDatasetDto addDatasetDto);
    Task<ServiceResponse<IEnumerable<GetDatasetPreviewDto>>> GetAllDatasets();
    Task<ServiceResponse<GetDitailedDatasetDto>> DeleteDataset(long datasetId);
    Task<ServiceResponse<GetDitailedDatasetDto>> GetSingleDataset(long datasetId);
    Task<ServiceResponse<GetGraphDto>> GetGraph(long datasetId, string vertexIdentifier, string sourceIdentifier, string targetIdentifier);
    Task<ServiceResponse<GetGraphDto>> GetSubGraph(GetSubGraphDto getSubGraphDto);
}