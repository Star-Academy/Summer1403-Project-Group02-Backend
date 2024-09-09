using mohaymen_codestar_Team02.CleanArch1.GraphService;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02_XUnitTest.CleanArch1;

public class GraphServiceTest
{
    private GraphService _sut;

    public GraphServiceTest()
    {
        _sut = new GraphService();
    }

    [Fact]
    public void GetGraph_ShouldReturnListOfVerticesAndDestinations_WhenGivenDatasetNameAndIdentifiers()
    {
        // Arrange
        long datasetId = 1;
        var vertexAttName1 = "VertexAttName1";
        var vertexAttName2 = "VertexAttName2";
        var edgeAttName1 = "EdgeAttName1";
        var edgeAttName2 = "EdgeAttName2";
        var edgeAttName3 = "EdgeAttName3";
        var sourceIdentifierFieldName = edgeAttName1;
        var targetIdentifierFieldName = edgeAttName2;
        var vertexIdentifierFieldName = vertexAttName1;

        var vertexAttributeValues = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "id1", new Dictionary<string, string>()
                {
                    { vertexAttName1, "val1" },
                    { vertexAttName2, "val2" }
                }
            },
            {
                "id2", new Dictionary<string, string>()
                {
                    { vertexAttName1, "val1" },
                    { vertexAttName2, "val3" }
                }
            },
            {
                "id3", new Dictionary<string, string>()
                {
                    { vertexAttName1, "val2" },
                    { vertexAttName2, "val2" }
                }
            }
        };

        var edgeAttributeValues = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "id1", new Dictionary<string, string>()
                {
                    { edgeAttName1, "val1" }, // s= 1, 2    d= 3
                    { edgeAttName2, "val2" },
                    { edgeAttName3, "val8" }
                }
            },
            {
                "id2", new Dictionary<string, string>()
                {
                    { edgeAttName1, "val2" }, // 
                    { edgeAttName2, "val4" },
                    { edgeAttName3, "val8" }
                }
            },
            {
                "id3", new Dictionary<string, string>()
                {
                    { edgeAttName1, "val2" }, // s= 3   d= 1, 2
                    { edgeAttName2, "val1" },
                    { edgeAttName3, "val8" }
                }
            }
        };

        var expectedVertex = new List<Vertex>()
        {
            new()
            {
                Id = "id1",
                Label = "val1"
            },
            new()
            {
                Id = "id2",
                Label = "val1"
            },
            new()
            {
                Id = "id3",
                Label = "val2"
            }
        };

        var expectedEdge = new List<Edge>()
        {
            new()
            {
                Id = "id1",
                Source = "id1",
                Target = "id3"
            },
            new()
            {
                Id = "id1",
                Source = "id2",
                Target = "id3"
            },
            new()
            {
                Id = "id3",
                Source = "id3",
                Target = "id1"
            },
            new()
            {
                Id = "id3",
                Source = "id3",
                Target = "id2"
            }
        };

        var expected = (expectedVertex, expectedEdge);

        // Act
        var actual = _sut.GetGraph(vertexAttributeValues, edgeAttributeValues, vertexIdentifierFieldName,
            sourceIdentifierFieldName, targetIdentifierFieldName);

        // Assert
        Assert.Equivalent(expected, actual);
    }
}