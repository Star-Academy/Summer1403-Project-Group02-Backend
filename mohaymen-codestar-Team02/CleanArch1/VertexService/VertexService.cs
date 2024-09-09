using mohaymen_codestar_Team02.CleanArch1.Repositories.IEdgeRepository.Abstraction;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Services.VertexService.Abstraction;

namespace mohaymen_codestar_Team02.Services.VertexService;

public class VertexService : IVertexService
{
    
    private readonly IVertexRepository _vertexRepository;

    public VertexService(IVertexRepository vertexRepository)
    {
        _vertexRepository = vertexRepository;
    }
    
    public async Task<Dictionary<string, Dictionary<string, string>>> FilterVertices(long dataSetId, Dictionary<string, string> vertexAttributeVales)
    {
        var vertexRecords = await _vertexRepository.GetDatasetVertices(dataSetId);

        try
        {
            var validVertexRecords = vertexRecords
                .Where(group =>
                    vertexAttributeVales.All(attr =>
                        group.Any(v => v.VertexAttribute.Name == attr.Key && v.StringValue == attr.Value)));

            var res = validVertexRecords.ToDictionary(x => x.Key,
                x => x.ToDictionary(g => g.VertexAttribute.Name, g => g.StringValue));

            return res;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }


    public DetailDto GetVertexDetails(string objId)
    {
        /*
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var validValue = context.VertexValues.Where(value => value.ObjectId.ToLower() == objId.ToLower()).ToList();
        var result = new DetailDto();
        foreach (var value in validValue)
            result.AttributeValue[context.VertexAttributes.Find(value.VertexAttributeId).Name] = value.StringValue;

        return result;
        */
        throw new NotImplementedException();
    }
}