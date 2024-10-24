using Core.Entities;

namespace Core.Interfaces;

public interface IUnitOfWork: IDisposable
{
    IGenericRepository<TEnitity> Repository<TEnitity>() where TEnitity : BaseEntity;
    Task<int> Complete();
}