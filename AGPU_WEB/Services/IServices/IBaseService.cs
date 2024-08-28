using AGPU_WEB.Models;

namespace AGPU_WEB.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }  
        Task<T> SendAsync<T>(APIRequest apiRequest); // for calling the api
    }
}
