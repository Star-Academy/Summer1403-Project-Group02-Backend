namespace mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;

public class AddDatasetDto
{
    public IFormFile EdgeFile { get; init; }
    public IFormFile VertexFile { get; init; }
    public string DatasetName { get; init; }
    public string VertexEntityName { get; init; }
    public string EdgeEntityName { get; init; }
    public long UserId { get; init; }
}