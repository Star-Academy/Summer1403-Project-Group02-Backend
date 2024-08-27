using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.EdgeEAV;
using mohaymen_codestar_Team02.Models.VertexEAV;
using mohaymen_codestar_Team02.Services;

namespace mohaymen_codestar_Team02_XUnitTest.Servies;

public class EdgeServiceTest
{
    private IServiceProvider _serviceProvider;
    private EdgeService _sut;

    public EdgeServiceTest()
    {
        var serviceCollection = new ServiceCollection();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        serviceCollection.AddScoped(_ => new DataContext(options));

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _sut = new EdgeService(_serviceProvider);
    }

    [Fact]
    public void GetEdgeDetails_ReturnsCorrectDetails()
    {
        //Arange
        var objectId1 = "object1";
        var objectId2 = "object2";
        using var scope = _serviceProvider.CreateScope();
        var mockContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        var att1 = new EdgeAttribute("att1", 1);
        var att2 = new EdgeAttribute("att2", 2);
        att1.Id = 1;
        att2.Id = 2;

        List<EdgeAttribute> att = new() { att1, att2 };

        foreach (var attribute in att) mockContext.Add(attribute);

        var val1 = new EdgeValue("val1", 1, objectId2);
        var val2 = new EdgeValue("val2", 1, objectId1);
        var val3 = new EdgeValue("val3", 2, objectId1);
        var val4 = new EdgeValue("val4", 2, objectId2);
        List<EdgeValue> vertexValues = new() { val1, val2, val3, val4 };
        foreach (var value in vertexValues) mockContext.Add(value);

        mockContext.SaveChanges();
        var expected = new Dictionary<string, string>();
        expected[att1.Name] = val2.StringValue;
        expected[att2.Name] = val3.StringValue;
        //Action
        var result = _sut.GetEdgeDetails(objectId1);
        //assert
        Assert.Equal(result.AttributeValue, expected);
    }

    [Fact]
    public void GetAllEdges_ShouldReturnAllEdges_WhenGivenCorrectDatasetAndIdentifiersName()
    {
        using var scope = _serviceProvider.CreateScope();
        var _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        // Arrange
        var datasetName = "DataSet1";
        var vertexIdentifierFieldName = "CardID";
        var sourceEdgeIdentifierFieldName = "SourceAcount";
        var destinationEdgeIdentifierFieldName = "DestiantionAccount";

        var expected = new List<Edge>()
        {
            new()
            {
                Id = "id1",
                Source = "id2",
                Target = "id3"
            },
            new()
            {
                Id = "id2",
                Source = "id4",
                Target = "id5"
            }
        };

        var dataset = new DataGroup("Dataset1", 1)
        {
            Name = "DataSet1",
            EdgeEntity = new EdgeEntity("Transaction", 1)
            {
                EdgeAttributes = new List<EdgeAttribute>
                {
                    new("SourceAcount", 1)
                    {
                        EdgeValues = new List<EdgeValue>
                        {
                            new("value1", 1, "id1") { EdgeAttribute = new EdgeAttribute("SourceAcount", 1) },
                            new("value2", 1, "id2") { EdgeAttribute = new EdgeAttribute("SourceAcount", 1) }
                        }
                    },
                    new("DestiantionAccount", 2)
                    {
                        EdgeValues = new List<EdgeValue>
                        {
                            new("value3", 2, "id1") { EdgeAttribute = new EdgeAttribute("DestiantionAccount", 1) },
                            new("value3", 2, "id2") { EdgeAttribute = new EdgeAttribute("DestiantionAccount", 1) }
                        }
                    }
                }
            },
            VertexEntity = new VertexEntity("Account", 1)
            {
                VertexAttributes = new List<VertexAttribute>
                {
                    new("CardID", 1)
                    {
                        VertexValues = new List<VertexValue>
                        {
                            new("value1", 1, "id2") { VertexAttribute = new VertexAttribute("CardID", 1) },
                            new("value3", 1, "id3") { VertexAttribute = new VertexAttribute("CardID", 1) },
                            new("value2", 1, "id4") { VertexAttribute = new VertexAttribute("CardID", 1) },
                            new("value3", 1, "id5") { VertexAttribute = new VertexAttribute("CardID", 1) }
                        }
                    }
                }
            }
        };

        _dataContext.DataSets.Add(dataset);
        _dataContext.SaveChanges();


        // Act
        var actual = _sut.GetAllEdges(datasetName, vertexIdentifierFieldName, sourceEdgeIdentifierFieldName,
            destinationEdgeIdentifierFieldName);

        // Assert
        Assert.Equivalent(expected, actual);
    }
}