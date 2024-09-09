using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset;
using mohaymen_codestar_Team02.CleanArch1.Dtos.Dataset.Attributes;
using mohaymen_codestar_Team02.CleanArch1.Services.DatasetService;
using mohaymen_codestar_Team02.Dto;
using mohaymen_codestar_Team02.Dto.GraphDTO;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.EdgeEAV;
using mohaymen_codestar_Team02.Models.VertexEAV;
using mohaymen_codestar_Team02.Services;
using NSubstitute;
using WebApplication15.Repositories;
using WebApplication15.Services;

namespace mohaymen_codestar_Team02_XUnitTest.CleanArch1;

public class DatasetServiceTest
{
    private readonly IDatasetService _sut;
    private readonly IDatasetRepositry _datasetRepositry;
    private readonly IVertexService _vertexService;
    private readonly IEdgeService _edgeService;
    private readonly IGraphService _graphService;
    private readonly IMapper _mapper;

    public DatasetServiceTest()
    {
        _datasetRepositry = Substitute.For<IDatasetRepositry>();
        _vertexService = Substitute.For<IVertexService>();
        _edgeService = Substitute.For<IEdgeService>();
        _graphService = Substitute.For<IGraphService>();
        _mapper = Substitute.For<IMapper>();
        _sut = new DatasetService(_datasetRepositry, _mapper, _vertexService, _edgeService, _graphService);
    }
    
    [Fact]
    public async Task GetSubGraph_ShouldReturnSubGraph_WhenGivenGetSubGraphDto()
    {
        // Arrange
        var vertices = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "objId1", new Dictionary<string, string>()
                {
                    {"VAtt1", "Val1"},
                    {"VAtt2", "Val2"},
                    {"VAtt3", "Val3"}
                }
            },
            {
                "objId2", new Dictionary<string, string>()
                {
                    {"VAtt1", "Val1"},
                    {"VAtt2", "Val2"},
                    {"VAtt3", "Val6"}
                }
            },
            {
                "objId3", new Dictionary<string, string>()
                {
                    {"VAtt1", "Val3"},
                    {"VAtt2", "Val8"},
                    {"VAtt3", "Val9"}
                }
            }
        };
        var edges = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "objId3", new Dictionary<string, string>()
                {
                    {"EAtt1", "Val1"},
                    {"EAtt2", "Val4"},
                    {"EAtt3", "Val3"}
                }
            },
            {
                "objId4", new Dictionary<string, string>()
                {
                    {"EAtt1", "Val4"},
                    {"EAtt2", "Val1"},
                    {"EAtt3", "Val3"}
                }
            }
        };

        var graphVertices = new List<Vertex>()
        {
            new Vertex()
            {
                Id = "objId1",
                Label = "Val1"
            },
            new Vertex()
            {
                Id = "objId2",
                Label = "Val2"
            }
        };

        var graphEdges = new List<Edge>()
        {
            new Edge()
            {
                Id = "objId3",
                Source = "objId1",
                Target = "objId2"
            }
        };

        var vertexAttValues = new Dictionary<string, string>()
        {
            {"VAtt1", "Val1"},
            {"VAtt2", "Val2"}
        };
        
        var edgeAttValues = new Dictionary<string, string>()
        {
            {"EAtt1", "Val1"}
        };

        var dto = new GetSubGraphDto()
        {
            DatasetId = 1,
            EdgeAttributeValues = edgeAttValues,
            SourceIdentifier = "EAtt1",
            TargetIdentifier = "EAtt2",
            VertexIdentifier = "VAtt1",
            VertexAttributeValues = vertexAttValues
        };

        _vertexService.FilterVertices(1, vertexAttValues)
            .Returns(Task.FromResult(vertices));

        _edgeService.FilterEdges(1, edgeAttValues)
            .Returns(Task.FromResult(edges));

        _graphService.GetGraph(vertices, edges, "VAtt1", "EAtt1", "EAtt2").Returns((graphVertices, graphEdges));

        var expected = new ServiceResponse<GetGraphDto>(new GetGraphDto()
        {
            Vertices = graphVertices,
            Edges = graphEdges
        }, ApiResponseType.Success, "");
        
        // Act
        var actual = await _sut.GetSubGraph(dto);
        
        // Assert
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async Task GetSingleDataset_ShouldReturnASingleDataset_WhenGivenDatasetId()
    {
        // Arrange
        var dataset = new DataGroup()
        {
            DataGroupId = 1,
            Name = "Dataset1",
            CreateAt = DateTime.MinValue,
            UpdateAt = DateTime.MaxValue,
            VertexEntity = new VertexEntity()
            {
                VertexEntityId = 1,
                Name = "VE1",
                VertexAttributes = new List<VertexAttribute>()
                {
                    new VertexAttribute()
                    {
                        Id = 1,
                        Name = "VA1"
                    },
                    new VertexAttribute()
                    {
                        Id = 2,
                        Name = "VA2"
                    }
                }
            },
            EdgeEntity = new EdgeEntity()
            {
                EdgeEntityId = 1,
                Name = "EE1",
                EdgeAttributes = new List<EdgeAttribute>()
                {
                    new EdgeAttribute()
                    {
                        Id = 1,
                        Name = "EA1"
                    },
                    new EdgeAttribute()
                    {
                        Id = 2,
                        Name = "EA2"
                    }
                }
            }
        };

        var datasetDto = new GetDitailedDatasetDto()
        {
            DataGroupId = 1,
            Name = "Dataset1",
            CreateAt = DateTime.MinValue,
            UpdateAt = DateTime.MaxValue,
            VertexEntity = new GetVertexEntityDto()
            {
                VertexEntityId = 1,
                Name = "VE1"
            },
            EdgeEntity = new GetEdgeEntityDto()
            {
                EdgeEntityId = 1,
                Name = "EE1"
            },
            VertexAttributes = new List<GetVertexAttributeDto>()
            {
                new GetVertexAttributeDto()
                {
                    Id = 1,
                    Name = "VA1"
                },
                new GetVertexAttributeDto()
                {
                    Id = 2,
                    Name = "VA2"
                }
            },
            EdgeAttributes = new List<GetEdgeAttributeDto>()
            {
                new GetEdgeAttributeDto()
                {
                    Id = 1,
                    Name = "EA1"
                },
                new GetEdgeAttributeDto()
                {
                    Id = 2,
                    Name = "EA2"
                }
            }
        };
        
        _datasetRepositry.GetSingleDataset(1).Returns(Task.FromResult(dataset));
        _mapper.Map<GetDitailedDatasetDto>(dataset).Returns(datasetDto);

        var expected = new ServiceResponse<GetDitailedDatasetDto>(datasetDto, ApiResponseType.Success, "");
        
        // Act
        var actual = await _sut.GetSingleDataset(1);
        
        // Assert
        Assert.Equivalent(expected, actual);
    }
    
    [Fact]
    public async Task GetGraph_ShouldReturnGraph_WhenGivenCorrectInputs()
    {
        // Arrange
        var vertices = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "objId1", new Dictionary<string, string>()
                {
                    {"VAtt1", "Val1"},
                    {"VAtt2", "Val2"},
                    {"VAtt3", "Val3"}
                }
            },
            {
                "objId2", new Dictionary<string, string>()
                {
                    {"VAtt1", "Val4"},
                    {"VAtt2", "Val5"},
                    {"VAtt3", "Val6"}
                }
            }
        };
        var edges = new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "objId3", new Dictionary<string, string>()
                {
                    {"EAtt1", "Val1"},
                    {"EAtt2", "Val4"},
                    {"EAtt3", "Val3"}
                }
            },
            {
                "objId4", new Dictionary<string, string>()
                {
                    {"EAtt1", "Val4"},
                    {"EAtt2", "Val1"},
                    {"EAtt3", "Val3"}
                }
            }
        };

        var graphVertices = new List<Vertex>()
        {
            new Vertex()
            {
                Id = "objId1",
                Label = "VVal1"
            },
            new Vertex()
            {
                Id = "objId2",
                Label = "VVal2"
            }
        };

        var graphEdges = new List<Edge>()
        {
            new Edge()
            {
                Id = "objId3",
                Source = "objId1",
                Target = "objId2"
            },
            new Edge()
            {
                Id = "objId4",
                Source = "objId2",
                Target = "objId1"
            }
        };

        _vertexService.FilterVertices(1, Arg.Any<Dictionary<string, string>>())
            .Returns(Task.FromResult(vertices));

        _edgeService.FilterEdges(1, Arg.Any<Dictionary<string, string>>())
            .Returns(Task.FromResult(edges));

        _graphService.GetGraph(vertices, edges, "VAtt1", "EAtt1", "EAtt2").Returns((graphVertices, graphEdges));

        var expected = new ServiceResponse<GetGraphDto>(new GetGraphDto()
        {
            Vertices = graphVertices,
            Edges = graphEdges
        }, ApiResponseType.Success, "");
        
        // Act
        var actual = await _sut.GetGraph(1, "VAtt1", "EAtt1", "EAtt2");
        
        // Assert
        Assert.Equivalent(expected, actual);
    }
    
    
    [Fact]
    public async Task GetAllDatasets_ShouldReturnAllDatasets_WhenExist()
    {
        // Arrange
        IEnumerable<DataGroup> datasets = new List<DataGroup>()
        {
            new DataGroup()
            {
                DataGroupId = 1,
                Name = "Dataset1",
                UserId = 2,
                CreateAt = DateTime.MinValue,
                UpdateAt = DateTime.MaxValue
            },
            new DataGroup()
            {
                DataGroupId = 2,
                Name = "Dataset2",
                UserId = 3,
                CreateAt = DateTime.MinValue,
                UpdateAt = DateTime.MaxValue
            }
        };
        
                
        var datasetDtos = new List<GetDatasetPreviewDto> 
        {
            new GetDatasetPreviewDto { 
                DataGroupId = 1,
                Name = "Dataset1",
                CreateAt = DateTime.MinValue,
                UpdateAt = DateTime.MaxValue
            },
            new GetDatasetPreviewDto
            {
                DataGroupId = 2,
                Name = "Dataset2",
                CreateAt = DateTime.MinValue,
                UpdateAt = DateTime.MaxValue
            }
        };
        
        _datasetRepositry.GetAllDatasets().Returns(Task.FromResult(datasets.AsEnumerable()));
        _mapper.Map<GetDatasetPreviewDto>(Arg.Any<DataGroup>()).Returns(args => 
            datasetDtos.FirstOrDefault(d => d.DataGroupId == ((DataGroup)args[0]).DataGroupId));

        var expected = new ServiceResponse<IEnumerable<GetDatasetPreviewDto>>(datasetDtos, ApiResponseType.Success, "");
        
        // Act
        var actual =await _sut.GetAllDatasets();
        
        // Assert
        Assert.Equivalent(expected, actual);
    }

    
}