namespace WebApplication15.Dtos.Dataset;

public class AddDatasetDto
{
    public IFormFile EdgeFile { get; init; }
    public IFormFile VertexFile { get; init; }
    public string DataName { get; init; }
}