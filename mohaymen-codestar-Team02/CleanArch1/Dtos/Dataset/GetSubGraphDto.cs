namespace mohaymen_codestar_Team02.Dto;

public class GetSubGraphDto
{
    public long DatasetId { get; set; }
    public string SourceIdentifier { get; set; }
    public string TargetIdentifier { get; set; }
    public string VertexIdentifier { get; set; } 
    public Dictionary<string, string> VertexAttributeValues { get; set; }
    public Dictionary<string, string> EdgeAttributeValues { get; set; }
}