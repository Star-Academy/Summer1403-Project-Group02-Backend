using AutoMapper;
using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository.Abstraction;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;

namespace mohaymen_codestar_Team02.Services;

public class EdgeService : IEdgeService
{
    private readonly IEdgeRepository _edgeRepository;
    public EdgeService(IEdgeRepository edgeRepository)
    {
        _edgeRepository = edgeRepository;
    }

    /*
    public List<GetAttributeDto> GetEdgeAttributes(long edgeEntityId)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var edgeAttribuite = context.EdgeEntities.Include(ve => ve.EdgeAttributes)
            .FirstOrDefault(ve => ve.EdgeEntityId == edgeEntityId)
            ?.EdgeAttributes;

        return edgeAttribuite.Select(va => _mapper.Map<GetAttributeDto>(va)).ToList();
    }*/

    public async Task<Dictionary<string, Dictionary<string, string>>> FilterEdges(long dataSetId, Dictionary<string, string> edgeAttributeVales)
    {
        var edgeRecords = await _edgeRepository.GetDatasetVertices(dataSetId);
        var validEdgeRecords = edgeRecords
            .Where(group =>
                edgeAttributeVales.All(attr =>
                    group.Any(v => v.EdgeAttribute.Name == attr.Key && v.StringValue == attr.Value)));

        var res = validEdgeRecords.ToDictionary(x => x.Key,
            x => x.ToDictionary(g => g.EdgeAttribute.Name, g => g.StringValue));
        return res;
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