using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models.VertexEAV;
using mohaymen_codestar_Team02.Services;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.IVertexRepository;

public class VertexRepository : IEdgeRepository.Abstraction.IVertexRepository
{
    private readonly IServiceProvider _serviceProvider;

    public VertexRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<IGrouping<string, VertexValue>>> GetDatasetVertices(long dataSetId)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var vertexEntity = await context.VertexEntities.Where(ds => ds.DataGroupId == dataSetId)
            .Include(ve => ve.VertexAttributes).ThenInclude(vv => vv.VertexValues)
            .FirstOrDefaultAsync();

        return vertexEntity.VertexAttributes.Select(a => a.VertexValues).SelectMany(v => v)
            .GroupBy(v => v.ObjectId);
    }
}