using mohaymen_codestar_Team02.Models.EdgeEAV;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.EdgeRepository.Abstraction;

public interface IEdgeRepository
{
    Task<IEnumerable<IGrouping<string, EdgeValue>>> GetDatasetVertices(long dataSetId);
}