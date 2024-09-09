using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;
using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Services;
using WebApplication15.Repositories;
using WebApplication15.Services;

namespace mohaymen_codestar_Team02.CleanArch1.Services.DatasetService;

public class DatasetService : IDatasetService
{
    private readonly IDatasetRepositry _datasetRepositry;
    private readonly IVertexService _vertexService;
    private readonly IEdgeService _edgeService;
    private readonly IGraphService _graphService;
    private readonly IMapper _mapper;

    public DatasetService(IDatasetRepositry datasetRepositry, IMapper mapper, IVertexService vertexService, IEdgeService edgeService, IGraphService graphService)
    {
        _datasetRepositry = datasetRepositry;
        _mapper = mapper;
        _vertexService = vertexService;
        _edgeService = edgeService;
        _graphService = graphService;
    }
    
    public async Task<ServiceResponse<GetDitailedDatasetDto>> AddDataset(AddDatasetDto addDatasetDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<GetGraphDto>> GetSubGraph(GetSubGraphDto getSubGraphDto)
    {
        var vertices = await _vertexService.FilterVertices(getSubGraphDto.DatasetId, getSubGraphDto.VertexAttributeValues);
        var edges = await _edgeService.FilterEdges(getSubGraphDto.DatasetId, getSubGraphDto.EdgeAttributeValues);
        var graph = _graphService.GetGraph(vertices, edges, getSubGraphDto.VertexIdentifier, getSubGraphDto.SourceIdentifier,
            getSubGraphDto.TargetIdentifier);

        var dto = new GetGraphDto() // use mapper
        {
            Vertices = graph.vertices,
            Edges = graph.edges
        };
        
        return new ServiceResponse<GetGraphDto>(dto, ApiResponseType.Success,
            "");
    }
    
    public async Task<ServiceResponse<GetGraphDto>> GetGraph(long datasetId, string vertexIdentifier, string sourceIdentifier, string targetIdentifier)
    {
        var vertices = await _vertexService.FilterVertices(datasetId, new Dictionary<string, string>(){});
        var edges = await _edgeService.FilterEdges(datasetId, new Dictionary<string, string>(){});
        var graph = _graphService.GetGraph(vertices, edges, vertexIdentifier, sourceIdentifier,
            targetIdentifier);

        var dto = new GetGraphDto()
        {
            Vertices = graph.vertices,
            Edges = graph.edges
        };
        
        return new ServiceResponse<GetGraphDto>(dto, ApiResponseType.Success,
            "");
    }
    
    public async Task<ServiceResponse<IEnumerable<GetDatasetPreviewDto>>> GetAllDatasets()
    {
        var datasets = await _datasetRepositry.GetAllDatasets();
        
        var dataGroupDtos = datasets.Select(ds => _mapper.Map<GetDatasetPreviewDto>(ds)).ToList();
        return new ServiceResponse<IEnumerable<GetDatasetPreviewDto>>(dataGroupDtos, ApiResponseType.Success, "");
    }
    
    public async Task<ServiceResponse<GetDitailedDatasetDto>> GetSingleDataset(long datasetId)
    {
        var dataset = await _datasetRepositry.GetSingleDataset(datasetId);
        var datasetDto = _mapper.Map<GetDitailedDatasetDto>(dataset);
        return new ServiceResponse<GetDitailedDatasetDto>(datasetDto, ApiResponseType.Success, "");
    }
    
    public Task<ServiceResponse<DetailDto>> GetSingleVertex(string objId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<DetailDto>> GetSingleEdge(string objId)
    {
        throw new NotImplementedException();
    }
    
    public Task<ServiceResponse<GetDitailedDatasetDto>> DeleteDataset(long datasetId)
    {
        throw new NotImplementedException();
    }

}