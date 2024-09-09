using mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository.Abstraction;
using mohaymen_codestar_Team02.Dtos.Extra.GraphDto;

namespace mohaymen_codestar_Team02.Services.EdgeService;

public class EdgeService : IEdgeService
{
    private readonly IEdgeRepository _edgeRepository;
    public EdgeService(IEdgeRepository edgeRepository)
    {
        _edgeRepository = edgeRepository;
    }
    
    public async Task<Dictionary<string, Dictionary<string, string>>> FilterEdges(long dataSetId, Dictionary<string, string> edgeAttributeVales)
    {
        var edgeRecords = await _edgeRepository.GetDatasetVertices(dataSetId);

        try
        {
            var validEdgeRecords = edgeRecords
                .Where(group =>
                    edgeAttributeVales.All(attr =>
                        group.Any(v => v.EdgeAttribute.Name == attr.Key && v.StringValue == attr.Value)));

            var res = validEdgeRecords.ToDictionary(x => x.Key,
                x => x.ToDictionary(g => g.EdgeAttribute.Name, g => g.StringValue));
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }

    
    public DetailDto GetEdgeDetails(string objId)
    {
        /*
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var validValue = context.EdgeValues.Where(value => value.ObjectId.ToLower() == objId.ToLower()).ToList();
        var result = new DetailDto();
        foreach (var value in validValue)
            result.AttributeValue[context.EdgeAttributes.Find(value.EdgeAttributeId).Name] = value.StringValue;

        return result;
        */
        throw new NotImplementedException();
    }
}