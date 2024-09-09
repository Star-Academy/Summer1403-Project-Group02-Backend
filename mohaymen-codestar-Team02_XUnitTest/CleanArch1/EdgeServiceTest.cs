using mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository;
using mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository.Abstraction;
using mohaymen_codestar_Team02.Models.EdgeEAV;
using mohaymen_codestar_Team02.Services;
using NSubstitute;

namespace mohaymen_codestar_Team02_XUnitTest.CleanArch1;

public class EdgeServiceTest
{
    private readonly IEdgeService _sut;
    private readonly IEdgeRepository _edgeRepository;

    public EdgeServiceTest()
    {
        _edgeRepository = Substitute.For<IEdgeRepository>();
        _sut = new EdgeService(_edgeRepository);
    }

    [Fact]
    public async Task FilterEdges_ShouldReturnEdgesMatchingFilters_WhenGivenFilters()
    {
        // Arrange
        var datasetId = 1;
        var vertexAttributeVales = new Dictionary<string, string>()
        {
            {"EAtt1", "Val1"},
            {"EAtt2", "Val2"}
        };
        
        var vertexRecords = new List<IGrouping<string, EdgeValue>>
        {
            new Grouping<string, EdgeValue>("objId1", new List<EdgeValue>
            {
                new EdgeValue { EdgeAttribute = new EdgeAttribute { Name = "EAtt1" }, StringValue = "Val1" },
                new EdgeValue { EdgeAttribute = new EdgeAttribute { Name = "EAtt2" }, StringValue = "Val2" }
            }),
            new Grouping<string, EdgeValue>("objId2", new List<EdgeValue>
            {
                new EdgeValue { EdgeAttribute = new EdgeAttribute { Name = "EAtt1" }, StringValue = "Val3" },
                new EdgeValue { EdgeAttribute = new EdgeAttribute { Name = "EAtt2" }, StringValue = "Val4" }
            })
        };
        
        _edgeRepository.GetDatasetVertices(datasetId)
            .Returns(Task.FromResult((IEnumerable<IGrouping<string, EdgeValue>>)vertexRecords));
        
        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "objId1", new Dictionary<string, string>
                {
                    { "EAtt1", "Val1" },
                    { "EAtt2", "Val2" }
                }
            },
        };

        // Act
        var actual = await _sut.FilterEdges(datasetId, vertexAttributeVales);

        // Assert
        Assert.Equivalent(expected, actual);
    }

}