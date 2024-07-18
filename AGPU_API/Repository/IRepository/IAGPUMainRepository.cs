using AGPU_API.Models;

namespace AGPU_API.Repository.IRepository
{
    public interface IAGPUMainRepository : IAGPURepository<AGPU>
    {
        Task<AGPU> UpdateAsync(AGPU entity);
    }
}
