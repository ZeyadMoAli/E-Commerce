using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly StoreContext _context;
    private Hashtable _repositories;

    public UnitOfWork(StoreContext context)
    {
        _context = context;
    }
    public void Dispose()
    {
        _context.Dispose();
    }

    public IGenericRepository<TEnitity> Repository<TEnitity>() where TEnitity : BaseEntity
    {
        if(_repositories == null)
            _repositories = new Hashtable();
        
        var type = typeof(TEnitity).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEnitity)), _context);
            _repositories.Add(type, repositoryInstance);
        }
        return (IGenericRepository<TEnitity>)_repositories[type];
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }
}