using mohaymen_codestar_Team02.Models;

namespace WebApplication15.Repositories;

public interface IDatasetRepositry
{
    Task<IEnumerable<DataGroup>> GetAllDatasets();
    Task<DataGroup> GetSingleDataset(long id);
    Task<DataGroup> AddDataset(DataGroup dataset);
    Task DeleteDataset(long id);
}