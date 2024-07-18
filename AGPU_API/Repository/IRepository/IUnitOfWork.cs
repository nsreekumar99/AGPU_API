using AGPU_API.Models;

namespace AGPU_API.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IAGPURepository<AGPU> AGPUs { get; }
        IAGPUMainRepository AGPUMain { get; }
        Task SaveAsync();
    }
}
