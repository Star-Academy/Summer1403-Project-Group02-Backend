using mohaymen_codestar_Team02.CleanArch1.Repositories.IEdgeRepository.Abstraction;
using mohaymen_codestar_Team02.Models.VertexEAV;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.VertexService;
using mohaymen_codestar_Team02.Services.VertexService.Abstraction;
using NSubstitute;

namespace mohaymen_codestar_Team02_XUnitTest.CleanArch1;

public class VertexServiceTest
{
    private readonly IVertexService _sut;
    private readonly IVertexRepository _vertexRepository;

    public VertexServiceTest()
    {
        _vertexRepository = Substitute.For<IVertexRepository>();
        _sut = new VertexService(_vertexRepository);
    }

    public async Task<Dictionary<string, Dictionary<string, string>>> FilterVertices(long dataSetId, Dictionary<string, string> vertexAttributeVales)
    {
        var vertexRecords = await _vertexRepository.GetDatasetVertices(dataSetId);
        
        var validVertexRecords = vertexRecords
            .Where(group =>
                vertexAttributeVales.All(attr =>
                    group.Any(v => v.VertexAttribute.Name == attr.Key && v.StringValue == attr.Value)));

        var res = validVertexRecords.ToDictionary(x => x.Key,
            x => x.ToDictionary(g => g.VertexAttribute.Name, g => g.StringValue));

        return res;
    }

    [Fact]
    public async Task FilterVertices_ShouldReturnVerticesMatchingFilters_WhenGivenCorrectFilters()
    {
        // Arrange
        var datasetId = 1;
        var vertexAttributeVales = new Dictionary<string, string>()
        {
            {"VAtt1", "Val1"},
            {"VAtt2", "Val2"}
        };
        
        var vertexRecords = new List<IGrouping<string, VertexValue>>
        {
            new Grouping<string, VertexValue>("objId1", new List<VertexValue>
            {
                new VertexValue { VertexAttribute = new VertexAttribute { Name = "VAtt1" }, StringValue = "Val1" },
                new VertexValue { VertexAttribute = new VertexAttribute { Name = "VAtt2" }, StringValue = "Val2" }
            }),
            new Grouping<string, VertexValue>("objId2", new List<VertexValue>
            {
                new VertexValue { VertexAttribute = new VertexAttribute { Name = "VAtt1" }, StringValue = "Val3" },
                new VertexValue { VertexAttribute = new VertexAttribute { Name = "VAtt2" }, StringValue = "Val4" }
            })
        };
        
        _vertexRepository.GetDatasetVertices(datasetId)
            .Returns(Task.FromResult((IEnumerable<IGrouping<string, VertexValue>>)vertexRecords));
        
        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "objId1", new Dictionary<string, string>
                {
                    { "VAtt1", "Val1" },
                    { "VAtt2", "Val2" }
                }
            },
        };

        // Act
        var actual = await _sut.FilterVertices(datasetId, vertexAttributeVales);

        // Assert
        Assert.Equivalent(expected, actual);
    }
}