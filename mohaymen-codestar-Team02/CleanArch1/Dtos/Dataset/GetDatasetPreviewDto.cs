using mohaymen_codestar_Team02.Models;

namespace WebApplication15.Dtos.Dataset;

public class GetDatasetPreviewDto
{
    public long DataGroupId { get; set; }
    public string Name { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
}