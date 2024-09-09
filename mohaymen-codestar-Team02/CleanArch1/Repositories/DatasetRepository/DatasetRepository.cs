using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;
using WebApplication15.Repositories;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.DatasetRepository;

public class DatasetRepository : IDatasetRepositry
{
    
    private readonly IServiceProvider _serviceProvider;

    public DatasetRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task<DataGroup> AddDataset(DataGroup dataset)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var result = await context.DataSets.AddAsync(dataset);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<IEnumerable<DataGroup>> GetAllDatasets()
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        return await context.DataSets.ToListAsync();
    }

    public async Task<DataGroup> GetSingleDataset(long id)
    {
        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        return await context.DataSets
            .Where(ds => ds.DataGroupId == id)
            .Include(ds => ds.VertexEntity).ThenInclude(ve => ve.VertexAttributes)
            .Include(ds => ds.EdgeEntity).ThenInclude(ee => ee.EdgeAttributes).FirstOrDefaultAsync();
    }
    
    public Task DeleteDataset(long id)
    {
        throw new NotImplementedException();
    }
}