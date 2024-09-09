using mohaymen_codestar_Team02.Dtos.EdgeDtos;
using mohaymen_codestar_Team02.Dtos.VertexDtos;

namespace mohaymen_codestar_Team02.Dtos.Extra;

public class GetDataGroupDto
{
    public long Id { get; set; }
    public string Name { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    public virtual GetEdgeEntityDto EdgeEntity { get; set; }
    public virtual GetVertexEntityDto VertexEntity { get; set; }
}