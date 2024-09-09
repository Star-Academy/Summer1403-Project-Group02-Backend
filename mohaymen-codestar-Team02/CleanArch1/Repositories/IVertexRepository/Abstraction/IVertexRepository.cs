using mohaymen_codestar_Team02.Models.VertexEAV;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.IEdgeRepository.Abstraction;

public interface IVertexRepository
{ 
    Task<IEnumerable<IGrouping<string, VertexValue>>> GetDatasetVertices(long dataSetId);

    //Task<VertexEntity, IEnumerable<VertexAttribute>, IEnumerable<VertexValue>> AddVertices(VertexEntity vertexEntity, IEnumerable<VertexAttribute> vertexAttributes,
        //IEnumerable<VertexValue> vertexValues);
}