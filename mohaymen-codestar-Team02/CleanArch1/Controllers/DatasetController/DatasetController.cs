using Microsoft.AspNetCore.Mvc;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;
using mohaymen_codestar_Team02.CleanArch1.Services.DatasetService.Abstraction;
using mohaymen_codestar_Team02.Dto;

namespace mohaymen_codestar_Team02.CleanArch1.Controllers.DatasetController;

public class DatasetController : ControllerBase
{
    private readonly IDatasetService _datasetService;

    public DatasetController(IDatasetService datasetService)
    {
        _datasetService = datasetService;
    }

    [HttpPost("Dataset/AddDataset")]
    public async Task<IActionResult> AddDataSet([FromForm] AddDatasetDto request)
    {
        var response = await _datasetService.AddDataset(request);
        return StatusCode((int)response.Type, response);
    }
    
    [HttpGet("Dataset/GetDatasets")]
    public async Task<IActionResult> GetAllDatasets()
    {
        var response = await _datasetService.GetAllDatasets();
        return StatusCode((int)response.Type, response);
    }
    
    [HttpGet("Dataset/GetDataset{datasetId}")]  
    public async Task<IActionResult> GetDataset(long datasetId) 
    {
        var response = await _datasetService.GetSingleDataset(datasetId);
        return StatusCode((int)response.Type, response);
    }

    [HttpDelete("Dataset/DeleteDataset{datasetId}")]
    public async Task<IActionResult> DeleteDataset(long datasetId)
    {
        var response = await _datasetService.DeleteDataset(datasetId);
        return StatusCode((int)response.Type, response);
    }
    
    [HttpGet("Dataset/GetSingleVertex/{vertexId}")]
    public async Task<IActionResult> GetSingleVertex(string vertexId)
    {
        var respond = await _datasetService.GetSingleVertex(vertexId);
        return StatusCode((int)respond.Type, respond);
    }       

    [HttpGet("Dataset/GetSingleEdge/{edgeId}")]
    public async Task<IActionResult> GetSingleEdge(string edgeId)
    {
        var respond = await _datasetService.GetSingleEdge(edgeId);
        return StatusCode((int)respond.Type, respond);
    }

    [HttpGet("Dataset/GetGraph{datasetId}")]
    public async Task<IActionResult> GetModel(long datasetId, [FromQuery]string vertexIdentifier, [FromQuery]string sourceIdentifier, [FromQuery]string targetIdentifier) // from query
    {
        var response = await _datasetService.GetDataModel(datasetId, vertexIdentifier, sourceIdentifier, targetIdentifier);
        return StatusCode((int)response.Type, response);
    }

    [HttpPost("Dataset/FilterDataset")]
    public async Task<IActionResult> GetFilteredDataModel([FromBody]GetSubGraphDto request)
    {
        var response = await _datasetService.GetFilteredDataModel(request);
        response.Data.GraphId = request.DatasetId;
        return StatusCode((int)response.Type, response);
    }
}