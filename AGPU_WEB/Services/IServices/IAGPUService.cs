using AGPU_WEB.Models.DTO;
using System.Linq.Expressions;

namespace AGPU_WEB.Services.IServices
{
    public interface IAGPUService
    {
        Task<T> GetAsync<T>(int id);
        Task<T> GetAllAsync<T>();
        Task<T> AddAsync<T>(AGPUCreateDTO dto);
        Task<T> RemoveAsync<T>(int id); //no async method for Remove
        Task<T> UpdateAsync<T>(AGPUUpdateDTO dto);
    }
}
