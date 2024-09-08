using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset.Attributes;
using mohaymen_codestar_Team02.Dto;

namespace WebApplication15.Dtos.Dataset;

public class GetDitailedDatasetDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public virtual GetEdgeEntityDto EdgeEntity { get; set; }
    public virtual GetVertexEntityDto VertexEntity { get; set; }
    public virtual ICollection<GetVertexAttributeDto> VertexAttributes { get; set; }
    public virtual ICollection<GetEdgeAttributeDto> EdgeAttributes { get; set; }
}