using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models.EdgeEAV;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository;

public class EdgeRepository : Abstraction.IEdgeRepository
{
    private readonly IServiceProvider _serviceProvider;

    public EdgeRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<IGrouping<string, EdgeValue>>> GetDatasetVertices(long dataSetId)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var edgeEntity = await context.EdgeEntities.Where(ee => ee.DataGroupId == dataSetId)
            .Include(ee => ee.EdgeAttributes).ThenInclude(ev => ev.EdgeValues).FirstOrDefaultAsync();

        return edgeEntity.EdgeAttributes.Select(ea => ea.EdgeValues).SelectMany(v => v)
            .GroupBy(v => v.ObjectId);
    }
}